using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.Emails;
using Core.Entities.EnquiryAggregate;
using Core.Entities.HR;
using Core.Entities.Masters;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class CVRefService : ICVRefService
    {
        private readonly int _docControllerId = 3;
        private readonly IGenericRepository<CVRef> _cvrefRepo;
        private readonly IGenericRepository<CVForward> _cvFwdRepo;
        private readonly IGenericRepository<CVForwardItem> _cvFwdItemRepo;
        private readonly ATSContext _context;
        private readonly IGenericRepository<ToDo> _taskRepo;
        private readonly IEmailService _emailService;
        private readonly string _BCCmail = "admin@xyz.com";
        private readonly IEnquiryService _enqService;

        public IGenericRepository<Enquiry> _enqRepo { get; }
        public IGenericRepository<EnquiryItem> _enqItemRepo { get; }

        public class EandCList
        {
            public EandCList(int enquiryitemid, List<CList> candidateIds)
            {
                this.enquiryitemid = enquiryitemid;
                this.candidateids = candidateIds;
            }

            public int enquiryitemid { get; set; }
            public List<CList> candidateids { get; set; }
        }

        public class CList
        {
            public CList(int candidateid)
            {
                this.candidateid = candidateid;
            }
            public int candidateid { get; set; }
        }
        public CVRefService(IGenericRepository<CVRef> cvrefRepo, 
            IGenericRepository<CVForward> cvFwdRepo,
            IGenericRepository<CVForwardItem> cvFwdItemRepo, IGenericRepository<ToDo> taskRepo, 
            IGenericRepository<Enquiry> enqRepo, IGenericRepository<EnquiryItem> enqItemRepo,
            IEmailService emailService, IEnquiryService enqService, ATSContext context)
        {
            _enqService = enqService;
            _emailService = emailService;
            _taskRepo = taskRepo;
            _enqRepo = enqRepo;
            _enqItemRepo = enqItemRepo;
            _context = context;
            _cvrefRepo = cvrefRepo;
            _cvFwdRepo = cvFwdRepo;
            _cvFwdItemRepo = cvFwdItemRepo;
        }

//get CVRef
        public async Task<List<CVForward>> GetCVForwards(int enquiryId)
        {
            var ids = await _context.CVForwardItems.Where(x => x.EnquiryId==enquiryId)
                .Select(x=>x.CVForwardId).Distinct().ToListAsync();
            
            if (ids.Count==0) return null;

            var fwds = await _context.CVForwards.Include(x=>x.CVForwardItems)
                .Where(x => ids.Contains(x.Id)).ToListAsync();
            
            if (fwds==null || fwds.Count ==0) return null;

            return fwds;
        }

/*
        public async Task<List<CVForwardDto>> GetCVForwards(int enquiryno, DateTime? DateFrom, DateTime? DateUpto)
        {
            var fwds = await
            (
                from w in _context.CVForwards 
                join wi in _context.CVForwardItems on w.Id equals wi.CVForwardId
                join it in _context.EnquiryItems on wi.EnquiryItemId equals it.Id
                join e in _context.Enquiries on it.EnquiryId equals e.Id
                join c in _context.Categories on it.CategoryItemId equals c.Id
                join o in _context.CustomerOfficials on w.CustomerOfficialId equals o.Id
                join cnd in _context.Candidates on wi.CandidateId equals cnd.Id
                where e.Id == enquiryno
                orderby w.Id, wi.SrNo
                select(new { 
                    cvfwdid = w.Id,
                    customername = e.Customer.CustomerName + ", " + e.Customer.CustomerName,
                    officialname = o.Name + ", " + o.Designation,
                    officialemail = o.email,
                    mailsentref = w.MailSentRef,
                    orderdt = e.EnquiryDate,
                    catref = e.EnquiryNo + "-" + it.SrNo + "-" + c.Name,
                    srno = wi.SrNo,
                    appno = cnd.ApplicationNo,
                    candidatename = cnd.FullName,
                    ppno = cnd.PPNo,
                    //grade = wi.Grade,
                    //salaryexpect = wi.salaryexpectation,
                    //photo = wi.photoUrl,
                    dt = w.DateForwarded.Date,
                    cvrefid = wi.CVRefId,
                    senton = w
                })).ToListAsync();

            
            if (DateFrom != null && DateUpto !=null)
            {
                query = query.Where( x=> x.dt >= DateFrom.Value.Date && x.dt <= DateUpto.Value.Date);
            }
            
            var fwds = await query.ToListAsync();

            if (fwds==null || fwds.Count == 0) throw new Exception("No matching records found");

            int lastFwdId=0;
            int count=fwds.Count();
            int i=0;
            var cvforward = new List<CVForwardDto>();
            var listitems = new List<CVForwardItemDto>();
            
            int itemcvfwdid =0;
            DateTime itemdt=DateTime.Now;
            string itemcustomername="";
            string itemofficialname="";
            string itemmailsentref="";
            
            foreach(var item in fwds)
            {
                if (item.cvfwdid==lastFwdId)
                cvforward.Add(new CVForwardDto(item.cvfwdid, item.dt, item.customername,
                        item.officialname, item,officialname, item.mailsentref, item.));
                    //listitems=null;
                    listitems = new List<CVForwardItemDto>();

                }else 
                {
                    if(lastFwdId !=0) 
                    {    cvforward.Add(new CVForwardDto(item.cvfwdid, item.dt, item.customername,
                            item.officialname, item.officialname, item.mailsentref, listitems));
                    }
                }
                listitems.Add(new CVForwardItemDto(item.srno, item.catref, item.appno, 
                    item.candidatename, item.ppno));
                if (i >= count)
                {   cvforward.Add(new CVForwardDto(item.cvfwdid, item.dt, item.customername,
                        item.officialname, item.officialname, item.mailsentref, listitems));
                }
                lastFwdId=item.cvfwdid;

                itemcvfwdid = item.cvfwdid;
                itemdt=item.dt;
                itemcustomername=item.customername;
                itemofficialname=item.officialname;
                itemmailsentref=item.mailsentref;

            }
   
            return cvforward;

        }
*/
        public async Task<IReadOnlyList<CVForwardItem>> GetCVRefOfACandidate(int candidateId)
        {
            var cvfwd = await _cvFwdItemRepo.GetEntityListWithSpec(new CVForwardItemsSpecs(candidateId));
            return cvfwd;
        }

        public async Task<IReadOnlyList<CVForwardItem>> GetCVRefForEnquiryIdWithAllStatus(
            int enquiryId)
        {
            var itemsList = await _context.EnquiryItems.Where(x => x.EnquiryId == enquiryId)
                .Select(x => x.Id).ToListAsync();
            if (itemsList == null || itemsList.Count == 0) return null;

            var cvfwd = await _cvFwdItemRepo.GetEntityListWithSpec(
                new CVForwardItemsSpecs("dummy", enquiryId));
            
            return cvfwd;
        }

    // no cvforwarditemspecs provides for enquiryitemid
        public async Task<IReadOnlyList<CVForwardItem>> GetCVRefForEnquiryItem(int enquiryItemId)
        {
            var itemsList = new List<int>() { enquiryItemId };
            return await _cvFwdItemRepo.GetEntityListWithSpec(new CVForwardItemsSpecs(enquiryItemId));
        }

        public async Task<IReadOnlyList<CVForwardItem>> GetCVRefForEnquiryitemIdWithStatus(
            int enquiryId, enumSelectionResult result)
        {
            var itemsList = await _context.EnquiryItems.Where(x => x.EnquiryId == enquiryId)
                .Select(x => x.Id).ToListAsync();
            if (itemsList == null || itemsList.Count == 0) return null;

            return await _cvFwdItemRepo.GetEntityListWithSpec(new CVForwardItemsSpecs("dummy", enquiryId));
        }


        public async Task<IReadOnlyList<CVRef>> ValidateCVRefs(CVRefToAddDto cvrefdto, int userId)
        {
            //ensure all enquiry items belong to the same customer
           var custIds = new List<int>();
           foreach(var item in cvrefdto.CandidateAndEnqItemIds)
           {
               var enquiryId = await _context.EnquiryItems.Where(x => x.Id == item.enquiryItemId).Select(x=>x.EnquiryId).SingleOrDefaultAsync();
               if (enquiryId==0) throw new Exception("invalid enquiry item Id");
               item.enquiryId=enquiryId;
               var custid = await _context.Enquiries.Where(x => x.Id==enquiryId).Select(x=>x.CustomerId).SingleOrDefaultAsync();
               custIds.Add(custid);
           }
            var isDistinct = custIds.Distinct().AsEnumerable();
            if (isDistinct.Count() >1) throw new Exception ("Enquiry categories belong to more than one customer");
            int customerId=isDistinct.FirstOrDefault();

            // get the customer official id
            var offIds = await _context.CustomerOfficials
                .Where(x=>x.CustomerId==customerId && x.IsValid).OrderBy(x=>x.scope)
                .Select(x=>x.Id).ToListAsync();
            if (offIds==null) throw new Exception("Oficials of the customer not defined");
            if (!offIds.Contains(cvrefdto.CustomerOfficialId))
            {
                cvrefdto.CustomerOfficialId=offIds.FirstOrDefault();
            }

            cvrefdto.CustomerId=customerId;
            
            var cvRefsValidated = new List<CVRef>();
            var toDos = new List<ToDo>();
            var cands = new List<CVForwardCandidate>();
            int enqItemId = 0;
            var failedDto = new List<CandidateAndEnqItemId>();
            foreach (var cid in cvrefdto.CandidateAndEnqItemIds)
            {
                int id = cid.candidateID;
                enqItemId = cid.enquiryItemId;
                
                var app = await _context.Candidates.Where(x => x.Id == id)
                    .Select(x => new  { x.ApplicationNo, x.FullName, x.PassportNo, x.Gender, 
                    x.Email, x.AadharNo}).FirstOrDefaultAsync();
                if(app==null) throw new Exception("invalid candidate Id");
                
                if (!await IsCandidateReferred(id, enqItemId))
                {
                    cvRefsValidated.Add(new CVRef(cid.enquiryItemId, id, app.ApplicationNo, 
                        userId, cvrefdto.dateForwarded));
                }
                else
                {
                    throw new Exception("Candidate " + app.ApplicationNo + "(" + app.FullName +
                        " is already referred to requirement id " + enqItemId);
                    //failedDto.Add(new CandidateAndEnqItemId(id, cid.enquiryItemId));
                }
            }
            return cvRefsValidated;
        }

        //validates all cvref - those not vlaidated are not saved
        //creates CVForward - this acts as one set for all the cVRef items
        //this is also used by Document Controller for composing CV forward messages
        //after half an hour when the message is sent, the email system must send back
        //data to this application to update CVForward record, with success/failure value,
        //date sent and any other reference sent back by mailkit
        public async Task<CVForward> ReferCVsToClient(int userId, CVRefToAddDto cvrefdto)
        {

            var validatedCVRefs = await ValidateCVRefs(cvrefdto, userId);

            if (validatedCVRefs == null || validatedCVRefs.Count == 0) return null;

            var cvsReferred = await _cvrefRepo.AddListAsync(validatedCVRefs);
            if (cvsReferred == null || cvsReferred.Count == 0) return null;
        
            var fwd = new CVForward();

            fwd.DateForwarded=cvrefdto.dateForwarded;
            fwd.CustomerId=cvrefdto.CustomerId;
            fwd.CustomerOfficialId=cvrefdto.CustomerOfficialId;
            fwd.IncludeGrade=cvrefdto.includeGrade;
            fwd.IncludePhoto=cvrefdto.includePhoto;
            fwd.IncludeSalary=cvrefdto.includeSalaryExpectation;
            int srno=0;
            var fwditems = new List<CVForwardItem>();
            foreach(var item in cvsReferred)
            {
                fwditems.Add(new CVForwardItem(++srno, item.ApplicationNo, item.EnquiryItemId, item.EnquiryId, item.CandidateId, item.Id));
            }
            fwd.CVForwardItems=fwditems;
            var cvForwarded = await _cvFwdRepo.AddAsync(fwd);

            return cvForwarded;
        }


        //CVRef contains candidateid and enquiryItemId - unique index
        //flg returns CVRefId of the row where both these values are present
        private async Task<bool> IsCandidateReferred(int candidateId, int enquiryItemId)
        {
            return (await _context.CVRefs
                .Where(x => x.CandidateId == candidateId && x.EnquiryItemId == enquiryItemId)
                .Select(x => x.Id).FirstOrDefaultAsync() > 0);
        }


    // call by by messaging application.  Once a message is sent of type CVForward,
    // it calls back this function, which then updates the object CVForward, as well
    // as creates tasks for the project manager of respective enquiries.
        public async Task<bool> CallbackOnCVRefMessageSent(int response, int CVForwardId, DateTime TimeSent, string EmailRef)
        {
            var cvfwd = await _context.CVForwards.Where(x => x.Id == CVForwardId).FirstOrDefaultAsync();
            if (cvfwd==null) return false;

            cvfwd.DateForwarded=TimeSent;
            cvfwd.MailSentRef = EmailRef;
            cvfwd.SentMessageToClient=response;

            // create tasks for Project Manager to follow up for client selection
            var tasks = new List<ToDo>();

            var qryfwds = await (from fw in _context.CVForwards 
                join f in _context.CVForwardItems on fw.Id equals f.CVForwardId
                join i in _context.EnquiryItems on f.EnquiryItemId equals i.Id
                join c in _context.Candidates on f.CandidateId equals c.Id
                join e in _context.Enquiries on i.EnquiryId equals e.Id
                join cat in _context.Categories on i.CategoryItemId equals cat.Id
                where fw.Id==CVForwardId
                select new {customername=e.Customer.CustomerName, 
                    customercity=e.Customer.CityName, projmgrid=e.ProjectManagerId,
                    catref=e.EnquiryNo+"-"+i.SrNo+"-"+cat.Name, enquiryid=e.Id,
                    orderdate=e.EnquiryDate, appno=c.ApplicationNo, ppno=c.PassportNo,
                    candidatename=c.FullName, fwdon=fw.DateForwarded, 
                    enquiryitemid=i.Id}).ToListAsync();

            var dateNow = DateTime.Now;

            foreach(var item in qryfwds)
            {
                var description = "Application No. " + item.appno + " " + item.candidatename +
                ", PP No." + item.ppno + " forwarded to " + item.customername + ", " +
                item.customercity + " against their requirement " + item.catref +
                ". Please coordinate with client for their selections.";

                tasks.Add(new ToDo
                (_docControllerId, item.projmgrid, dateNow,
                    dateNow.AddDays(4), description, enumTaskType.Administrative, 
                    item.enquiryid, item.enquiryitemid
                ));
            }
            
            await _taskRepo.AddListAsync(tasks);

            return true;
        }

    //privates
    /*
    private async Task<EmailModel> ComposeEmailModelForCVForward(CVForward cvForward)
    {
        //compose email message
        string OfficialName = "";
        string OfficialEmail = "";
        int tot = 0;
        var enq = await _context.Enquiries.Where(x => x.Id == cvForward.EnquiryId)
            .Select(x => new { x.Id, x.CustomerId, x.EnquiryNo, x.EnquiryDate }).FirstOrDefaultAsync();

        var cust = await _context.Customers.Where(x => x.Id == cvForward.CustomerId)
            .Select(x => new { x.CustomerName, x.CityName, x.Email }).FirstOrDefaultAsync();
        var custOfficial = await _context.CustomerOfficials
            .Where(x => x.Id == cvForward.CustomerOfficialId && x.IsValid)
            .Select(x => new { x.Gender, x.Name, x.Designation, x.email, x.Mobile })
            .SingleOrDefaultAsync();
        if (custOfficial != null)
        {
            OfficialName = custOfficial.Gender == "M" ? "Mr." : "Ms. " +
                custOfficial.Name + "<br>" + custOfficial.Designation;
            if (!string.IsNullOrEmpty(custOfficial.email)) OfficialEmail = "<br>" +
             custOfficial.email;
        }

        var msg = cvForward.DateForwarded + "<br><br>M/S " + cust.CustomerName +
            "<br>" + cust.CityName + "<br>Kind Attn: " + OfficialName + "<br><br>" +
            "Dear " + custOfficial.Gender == "M" ? "Sir: " : "Madam: " + "<br><br>" +
            "With reference to your requirement dated " + enq.EnquiryDate.Date +
            " bearing our reference number " + enq.EnquiryNo + ", we are pleased to " +
            "forward herewith following CVs:<br><br>";

        foreach (var item in cvForward.CVForwardItems)
        {
            var enqItem = await _context.EnquiryItems.Where(x => x.Id == item.EnquiryItemId)
                .Select(x => new { x.CategoryItemId, x.CategoryName, x.SrNo, x.Quantity }).FirstOrDefaultAsync();

            string categoryName = "";

            if (string.IsNullOrEmpty(enqItem.CategoryName))
            {
                categoryName = await _context.Categories.Where(x => x.Id == enqItem.CategoryItemId)
                    .Select(x => x.Name).SingleOrDefaultAsync();
            }
            else
            {
                categoryName = enqItem.CategoryName;
            }

            var rejected = await _context.CVRefs.Where(x => x.EnquiryItemId == item.EnquiryItemId
                && (int)x.RefStatus >= 1000 && (int)x.RefStatus < 2000).CountAsync();
            var selected = await _context.CVRefs.Where(x => x.EnquiryItemId == item.EnquiryItemId
                && (int)x.RefStatus > 100 && (int)x.RefStatus <= 400).CountAsync();
            var referred = await _context.CVRefs
                .Where(x => x.EnquiryItemId == item.EnquiryItemId).CountAsync();
            int underreview = referred - selected - rejected;
            msg += "<br><table><th>Ref.No.</th><th>Category</th><th>Qnty<br>Reqd</th>" +
                "<th>Selected</th><th>Rejected</th><th>Under yr<br>review</th>" +
                "<th>total sent</th>" +
                "<td>" + enq.EnquiryNo + "-" + enqItem.SrNo + "</td><td>" + categoryName +
                "</td><td>" + enqItem.Quantity + "</td><td>" + selected + "</td><td>" + rejected +
                "</td><td>" + underreview + "</td><td>" + referred +
                "</td>/<table><br>CVs enclosed for above category<br>" +
                "<table><th>Sr<br>No</th><th>Application<br>Number</th><th>Candidate Name</th>" +
                "<th>PP No.</th>";
            if (cvForward.IncludePhoto) msg += "<th>Photograph</th>";
            if (cvForward.IncludeGrade) msg += "<th>Asessed<br>Grade</th>";
            if (cvForward.IncludeSalary) msg += "<th>Salary<br>Expectation</th>";

            int isrno = 0;
            foreach (var cvref in item.CVsRef)
            {
                tot += 1;
                isrno += 1;
                var app = await _context.Candidates.Where(x => x.Id == cvref.CandidateId)
                    .Select(x => new
                    {
                        x.ApplicationNo,
                        x.FullName,
                        x.PPNo,
                        x.Gender,
                        x.email,
                        x.AadharNo,
                        x.Attachments
                    })
                    .FirstOrDefaultAsync();
                msg += "<td>" + isrno + "</td><td>" + app.ApplicationNo + "</td><td>" + app.FullName +
                    "</td>td>" + app.PPNo + "</td>";
                if (cvForward.IncludePhoto)
                {
                    var photoUrl = await _context.Attachments.Where(x => x.CandidateId == cvref.CandidateId
                          && x.AttachmentType.ToLower() == "photo").Select(x => x.AttachmentUrl).FirstOrDefaultAsync();
                    msg += "<td>" + photoUrl + "</td>";
                }
                if (cvForward.IncludeGrade)
                {
                    var assessmentId = await _context.Assessments.Where(x => x.CandidateId == cvref.CandidateId
                          && x.EnquiryItemId == cvref.EnquiryItemId).Select(x => x.Id).FirstOrDefaultAsync();

                    var allotted = await _context.AssessmentItems.Where(x => x.Id == assessmentId
                          && x.Assessed == true).SumAsync(x => x.PointsAllotted);
                    //var assessmentterms = await _context.AssessmentItems.Where(x=>x.Id==assessmentId
                    //    && x.Assessed==true).reduce((a, b) => (b.PointsAllotted/b.MaxPoints)/a,0)
                    //    .FirstOrDefaultAsync(); 

                    var points = await _context.AssessmentItems.Where(x => x.Id == assessmentId
                          && x.Assessed == true).SumAsync(x => x.MaxPoints);

                    msg += "<td>" + 100 * allotted / points + "%" + "</td>";
                }
                if (cvForward.IncludeSalary)
                {
                    var jobcard = await _context.JobCards.Where(x => x.CandidateId == cvref.CandidateId
                          && x.EnquiryItemId == cvref.EnquiryItemId).Select(x => new
                          {
                              x.SalaryExpectCurrency,
                              x.SalaryExpectation
                          }).FirstOrDefaultAsync();
                    msg += "<td>" + jobcard.SalaryExpectCurrency + " " + jobcard.SalaryExpectation + "</td>";
                }
                msg += "<td></td>";
            }
        }

        //generate email messages
        var strEmailFrom = "munir.sheikh@live.com";
        var strEmailToList = new List<string>();
        var strCCList = new List<string>();
        var strBCCList = new List<string>();
        strEmailToList.Add(OfficialEmail);
        strBCCList.Add(_BCCmail);
        var strSubject = "Total " + tot + " candidates forwarded herewith against your requirement dated " +
            enq.EnquiryDate.Date;

        return new EmailModel(strEmailFrom, strEmailToList, strCCList,
            strBCCList, strSubject, null);
    }
    */

    private string GetPhotoUrl(int candidateId)
    {
        var photo = _context.Attachments.Where(x => x.CandidateId == candidateId &&
            x.AttachmentType == "photo").Select(x => x.AttachmentUrl).FirstOrDefault();
        if (string.IsNullOrEmpty(photo)) return "";
        return photo;
    }

    private string GetSalaryExpectation(int candidateId, int enquiryItemId)
    {
        var sal = _context.JobCards.Where(x => x.EnquiryItemId == enquiryItemId &&
            x.CandidateId == candidateId)
            .Select(x => new { x.SalaryExpectCurrency, x.SalaryExpectation })
            .FirstOrDefault();
        if (string.IsNullOrEmpty(sal.SalaryExpectCurrency))
        {
            return string.Empty;
        }
        else
        {
            return sal.SalaryExpectCurrency + " " + sal.SalaryExpectation;
        }
    }

    private string GetCountOfCVsForwarded(int enquiryItemId)
    {
        var ct = _context.CVRefs.Where(x => x.EnquiryItemId == enquiryItemId
            && (int)x.RefStatus < 1000).Count();
        var total = _context.EnquiryItems.Where(x => x.Id == enquiryItemId)
            .Select(x => x.Quantity).FirstOrDefault();
        return ct + "/" + total;

    }

    private string GetGrade(int candidateId, int enquiryItemId)
    {
        var grd = _context.Assessments.Where(x => x.EnquiryItemId == enquiryItemId &&
            x.CandidateId == candidateId).Select(x => x.Grade).FirstOrDefault();
        if (grd == 0) return "not assessed";
        return grd.ToString();
    }

        public Task<IReadOnlyList<CVForward>> GetCVsForwarded(CVForwardParam cvfwdParam)
        {
            throw new NotImplementedException();
        }
    }
}
