using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.Emails;
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
        private readonly ATSContext _context;
        private readonly IGenericRepository<ToDo> _taskRepo;
        private readonly IEmailService _emailService;
        private readonly string _BCCmail="admin@xyz.com";
        public CVRefService(IGenericRepository<CVRef> cvrefRepo, IGenericRepository<CVForward> cvFwdRepo,
            IGenericRepository<ToDo> taskRepo, IEmailService emailService, 
            ATSContext context)
        {
            _emailService = emailService;
            _taskRepo = taskRepo;
            _context = context;
            _cvrefRepo = cvrefRepo;
            _cvFwdRepo = cvFwdRepo;
        }

        public async Task<IReadOnlyList<CVRef>> GetCVRefOfACandidate(int candidateId)
        {
            return await _cvrefRepo.GetEntityListWithSpec(new CVRefSpecs(candidateId));
        }

        public async Task<IReadOnlyList<CVRef>> GetCVReferredForEnquiryIdWithAllStatus(
            int enquiryId)
        {
            var itemsList = await _context.EnquiryItems.Where(x => x.EnquiryId == enquiryId)
                .Select(x => x.Id).ToListAsync();
            if (itemsList == null || itemsList.Count == 0) return null;

            return await _cvrefRepo.GetEntityListWithSpec(new CVRefSpecs(itemsList));
        }

        public async Task<IReadOnlyList<CVRef>> GetCVReferredForEnquiryIdWithStatus(
            int enquiryId, enumSelectionResult result)
        {
            var itemsList = await _context.EnquiryItems.Where(x => x.EnquiryId == enquiryId)
                .Select(x => x.Id).ToListAsync();
            if (itemsList == null || itemsList.Count == 0) return null;

            return await _cvrefRepo.GetEntityListWithSpec(new CVRefSpecs(itemsList, result));
        }

        public async Task<IReadOnlyList<CVRef>> GetCVReferredForEnquiryItem(int enquiryItemId)
        {
            var itemsList = new List<int>() { enquiryItemId };
            return await _cvrefRepo.GetEntityListWithSpec(new CVRefSpecs(itemsList));
        }

        public async Task<IReadOnlyList<CVRef>> GetCVReferredForEnquiryitemIdWithStatus(
            int enquiryId, enumSelectionResult result)
        {
            var itemsList = await _context.EnquiryItems.Where(x => x.EnquiryId == enquiryId)
                .Select(x => x.Id).ToListAsync();
            if (itemsList == null || itemsList.Count == 0) return null;

            return await _cvrefRepo.GetEntityListWithSpec(new CVRefSpecs(itemsList, result));

        }

        public async Task<IReadOnlyList<CVRef>> ReferCVsToClient(int userId, int enquiryItemId, 
            List<int> candidateIds, DateTime dateForwarded, bool includeSalaryExpectation,
            bool includeGrade, bool includePhoto)
        {
            string OfficialName="";
            string OfficialEmail="";
            var enqItem = await _context.EnquiryItems.Where(x => x.Id == enquiryItemId)
                .Select(x => new { x.CategoryItemId, x.CategoryName, x.SrNo }).FirstOrDefaultAsync();

            var enq = await _context.Enquiries.Where(x => x.Id == enquiryItemId)
                .Select(x => new { x.Id, x.CustomerId, x.EnquiryNo, x.EnquiryDate }).FirstOrDefaultAsync();

            var cust = await _context.Customers.Where(x => x.Id == enq.CustomerId)
                .Select(x => new { x.CustomerName, x.CityName, x.Email }).FirstOrDefaultAsync();
            var custOfficial = await _context.CustomerOfficials
                .Where(x => x.CustomerId == enq.CustomerId && x.IsValid)
                .OrderBy(x => x.scope).ToListAsync();
            if(custOfficial != null)
            {
                OfficialName = custOfficial[0].Name + "<br>" + custOfficial[0].Designation;
                if(!string.IsNullOrEmpty(custOfficial[0].email)) OfficialEmail = "<br>" + custOfficial[0].email;
            }
            string categoryName = "";
            if (string.IsNullOrEmpty(enqItem.CategoryName))
            {
                categoryName = await _context.Categories.Where(x => x.Id == enqItem.CategoryItemId)
                    .Select(x => x.Name).SingleOrDefaultAsync();
            } else
            {
                categoryName = enqItem.CategoryName;
            }

            var refDetails = " is forwarded on " + dateForwarded + " to " +
                cust.CustomerName + " at " + cust.CityName + " against requirement No. " +
                enq.EnquiryNo + "-" + enqItem.SrNo + "-" + categoryName;

            var cvRefs = new List<CVRef>();
            var toDos = new List<ToDo>();
            var Msg = new CVForwardMessages();
            var cands = new List<CVForwardCandidate>();
            string sCandidateDetails="";
            string addresseeBody = dateForwarded.Date +"<br><br>" + OfficialName + OfficialEmail;

            for (int j = -1; j < candidateIds.Count; j++)
            {
                var app = await _context.Candidates.Where(x => x.Id == candidateIds[j])
                    .Select(x => new { x.ApplicationNo, x.FullName, x.PPNo, x.Gender, x.email, x.AadharNo })
                    .FirstOrDefaultAsync();
                if (j==0)
                {
                    if (candidateIds.Count==1) {
                        sCandidateDetails = (app.Gender=="M"?"Mr.":"Ms.") + app.FullName +
                        ", Application No."+ app.ApplicationNo + " bearing PP No." + app.PPNo;
                    }else {
                        sCandidateDetails = " total " + candidateIds.Count + " numbers";
                    }
                }
                cands.Add(new CVForwardCandidate(j+1, candidateIds[j], app.ApplicationNo, 
                    app.FullName, app.PPNo, app.AadharNo, GetGrade(candidateIds[j], enquiryItemId), 
                    GetPhotoUrl(candidateIds[j]), GetSalaryExpectation(candidateIds[j], enquiryItemId),
                    "", GetCountOfCVsForwarded(enquiryItemId)));
                cvRefs.Add(new CVRef(enquiryItemId, candidateIds[j], app.ApplicationNo,
                    userId, DateTime.Now));

                var description = "Application " + app.ApplicationNo + " - " + app.FullName +
                    ", holding PP No. " + app.PPNo + refDetails;
                var taskItems = new List<TaskItem>();
                taskItems.Add(new TaskItem(DateTime.Now, 1, description, false, DateTime.Now.AddDays(4)));
                // create tasks
                toDos.Add(new ToDo(_docControllerId, _docControllerId, dateForwarded, dateForwarded.AddDays(4),
                    description, enumTaskType.CVReferral, enq.Id, enquiryItemId, taskItems));
            }

            var cvRefToReturn = await _cvrefRepo.AddListAsync(cvRefs);
            var tasksAdded = await _taskRepo.AddListAsync(toDos);
            var categoryRef = enq.EnquiryNo + "-" + enqItem.SrNo + "-" + categoryName;
            Msg.CategoryRef = categoryRef;
            Msg.candidates=cands;
            Msg.includeGrade=includeGrade;
            Msg.includeSalary=includeSalaryExpectation;
            Msg.includePhoto=includePhoto;
            
            //generate email messages
            var strEmailFrom = "munir.sheikh@live.com";
            var strEmailToList = new List<string>();
            var strCCList = new List<string>();
            var strBCCList = new List<string>();
            strEmailToList.Add(OfficialEmail);
            strBCCList.Add(_BCCmail);
            var strSubject = sCandidateDetails + " forwarded herewith against your requirement dated " + 
                enq.EnquiryDate.Date + " ref " + categoryRef;   

            var emailToSend = new EmailModel(strEmailFrom, strEmailToList, strCCList,
                strBCCList, strSubject, null);

            var eMailSent = _emailService.SendCVForwardingMessage(emailToSend, addresseeBody, Msg);

            return cvRefs;
        }

        public async Task<CVForward> ReferCVsToForward(CVForward cvForward)
        {
            var cvFwdToReturn = new CVForward();
            var cvFwdItem = new CVForwardItem();
            var cvFwdItems = new List<CVForwardItem>();
            var cvsref = new List<CVRef>();
            var dtForwarded=DateTime.Now;
            
            //update cvref
            foreach(var item in cvForward.CVForwardItems)
            {
                foreach(var cvref in item.CVsRef)
                {
                    cvref.DateForwarded=dtForwarded;
                    cvref.RefStatus=enumSelectionResult.Referred;
                    cvref.StatusDate=dtForwarded;
                    cvsref.Add(cvref);
                }
            }
            // before SaveAsync(cvsref), verify all enquiryitems belong to the same customerId
            var enquiryItemIdList = cvsref.Select(x=>x.EnquiryItemId).Distinct().ToList();
            //verify all enquiryitemIDs belong to same customer
            var enquiryIdList = await _context.EnquiryItems
                .Where(x=>enquiryItemIdList.Contains(x.Id)).Select(x=>x.EnquiryId).Distinct().ToListAsync();
            if (enquiryIdList==null) return null; //
            var customerId = await _context.Enquiries.Where(x=>x.Id==enquiryIdList[0])
                .Select(x=>x.CustomerId).Distinct().FirstOrDefaultAsync();  //find distinct customerIds belonging to the enquiryitems
            if(customerId != cvForward.CustomerId) return null;

            //check the customerofficialId belongs to the same customer
            var off = await _context.CustomerOfficials.Where(x => x.Id==cvForward.CustomerOfficialId
                && x.IsValid && x.CustomerId==customerId).FirstOrDefaultAsync();
            if (off==null) return null;

            //Everything Ok, now save the cvsref part of CVForward
            var CVRefAdded = await _cvrefRepo.AddListAsync(cvsref);
        
            //create CVForward object to save
            cvFwdToReturn.CustomerId=customerId;
            cvFwdToReturn.CustomerOfficialId=cvForward.CustomerOfficialId;
            cvFwdToReturn.DateForwarded=dtForwarded;
            foreach(var itemId in enquiryItemIdList)
            {
                var cvRefsOfEnquiryItem = cvsref.Where(x=>x.EnquiryItemId==itemId).ToList();
                var cvForwardItem=new CVForwardItem(itemId, cvRefsOfEnquiryItem);
                cvFwdToReturn.CVForwardItems.Add(cvForwardItem);
            }
            var CVFwdObjToReturn = await _cvFwdRepo.AddAsync(cvFwdToReturn);

            if (cvForward.SendMessageToClient)
            {
                var emailModel = await ComposeEmailModelForCVForward(cvForward);
                if (emailModel !=null) 
                {
                    var emailSent = _emailService.SendEmail(emailModel);
                }
            }

            return CVFwdObjToReturn;
        }


    //privates

        private async Task<EmailModel> ComposeEmailModelForCVForward(CVForward cvForward)
        {
                        //compose email message
            string OfficialName="";
            string OfficialEmail="";
            int tot=0;
            var enq = await _context.Enquiries.Where(x => x.Id == cvForward.EnquiryId)
                .Select(x => new { x.Id, x.CustomerId, x.EnquiryNo, x.EnquiryDate }).FirstOrDefaultAsync();

            var cust = await _context.Customers.Where(x => x.Id == cvForward.CustomerId)
                .Select(x => new { x.CustomerName, x.CityName, x.Email }).FirstOrDefaultAsync();
            var custOfficial = await _context.CustomerOfficials
                .Where(x => x.Id == cvForward.CustomerOfficialId && x.IsValid)
                .Select(x => new {x.Gender, x.Name, x.Designation, x.email, x.Mobile})
                .SingleOrDefaultAsync();
            if(custOfficial != null)
            {
                OfficialName = custOfficial.Gender == "M" ? "Mr." : "Ms. " + 
                    custOfficial.Name + "<br>" + custOfficial.Designation;
                if(!string.IsNullOrEmpty(custOfficial.email)) OfficialEmail = "<br>" + 
                custOfficial.email;
            }
        
            var msg = cvForward.DateForwarded + "<br><br>M/S " + cust.CustomerName + 
                "<br>" + cust.CityName+ "<br>Kind Attn: " + OfficialName + "<br><br>" +
                "Dear " + custOfficial.Gender=="M" ? "Sir: " : "Madam: " + "<br><br>" +
                "With reference to your requirement dated " + enq.EnquiryDate.Date +
                " bearing our reference number " + enq.EnquiryNo + ", we are pleased to " +
                "forward herewith following CVs:<br><br>";
            
            foreach(var item in cvForward.CVForwardItems)
            {
                var enqItem = await _context.EnquiryItems.Where(x => x.Id == item.EnquiryItemId)
                    .Select(x => new { x.CategoryItemId, x.CategoryName, x.SrNo, x.Quantity }).FirstOrDefaultAsync();
                
                string categoryName = "";
                
                if (string.IsNullOrEmpty(enqItem.CategoryName))
                {
                    categoryName = await _context.Categories.Where(x => x.Id == enqItem.CategoryItemId)
                        .Select(x => x.Name).SingleOrDefaultAsync();
                } else
                {
                    categoryName = enqItem.CategoryName;
                }

                var rejected = await _context.CVRefs.Where(x => x.EnquiryItemId==item.EnquiryItemId
                    && (int)x.RefStatus>=1000 && (int)x.RefStatus<2000).CountAsync();
                var selected = await _context.CVRefs.Where(x => x.EnquiryItemId==item.EnquiryItemId
                    && (int)x.RefStatus>100 && (int)x.RefStatus<=400).CountAsync();
                var referred = await _context.CVRefs
                    .Where(x => x.EnquiryItemId==item.EnquiryItemId).CountAsync();
                int underreview = referred-selected-rejected;
                msg += "<br><table><th>Ref.No.</th><th>Category</th><th>Qnty<br>Reqd</th>" +
                    "<th>Selected</th><th>Rejected</th><th>Under yr<br>review</th>" +
                    "<th>total sent</th>"+
                    "<td>"+enq.EnquiryNo + "-" + enqItem.SrNo + "</td><td>" + categoryName + 
                    "</td><td>" + enqItem.Quantity + "</td><td>" + selected + "</td><td>" + rejected +
                    "</td><td>" + underreview + "</td><td>" + referred +
                    "</td>/<table><br>CVs enclosed for above category<br>" +
                    "<table><th>Sr<br>No</th><th>Application<br>Number</th><th>Candidate Name</th>" +
                    "<th>PP No.</th>";
                if(cvForward.IncludePhoto) msg +="<th>Photograph</th>";
                if(cvForward.IncludeGrade) msg +="<th>Asessed<br>Grade</th>";
                if(cvForward.IncludeSalary) msg +="<th>Salary<br>Expectation</th>";

                int isrno=0;
                foreach(var cvref in item.CVsRef)
                {
                    tot+=1;
                    isrno+=1;
                    var app = await _context.Candidates.Where(x => x.Id == cvref.CandidateId)
                        .Select(x => new { x.ApplicationNo, x.FullName, x.PPNo, x.Gender, 
                        x.email, x.AadharNo, x.Attachments })
                        .FirstOrDefaultAsync();
                    msg +="<td>"+isrno+"</td><td>"+app.ApplicationNo+"</td><td>"+app.FullName +
                        "</td>td>"+app.PPNo+"</td>";
                    if (cvForward.IncludePhoto) 
                    {
                        var photoUrl=await _context.Attachments.Where(x=>x.CandidateId==cvref.CandidateId
                            && x.AttachmentType.ToLower()=="photo").Select(x=>x.AttachmentUrl).FirstOrDefaultAsync();
                        msg+="<td>" + photoUrl + "</td>";
                    }
                    if(cvForward.IncludeGrade) 
                    {
                        var assessmentId=await _context.Assessments.Where(x=>x.CandidateId==cvref.CandidateId
                            && x.EnquiryItemId==cvref.EnquiryItemId).Select(x=>x.Id).FirstOrDefaultAsync();
                        
                        var allotted=await _context.AssessmentItems.Where(x => x.Id == assessmentId 
                            && x.Assessed==true).SumAsync(x=> x.PointsAllotted);
                        /*var assessmentterms = await _context.AssessmentItems.Where(x=>x.Id==assessmentId
                            && x.Assessed==true).reduce((a, b) => (b.PointsAllotted/b.MaxPoints)/a,0)
                            .FirstOrDefaultAsync(); */

                        var points=await _context.AssessmentItems.Where(x => x.Id == assessmentId
                            && x.Assessed==true).SumAsync(x=> x.MaxPoints);
                        
                        msg+="<td>" + 100*allotted/points + "%" + "</td>";
                    }
                    if(cvForward.IncludeSalary)
                    {
                        var jobcard=await _context.JobCards.Where(x=>x.CandidateId==cvref.CandidateId 
                            && x.EnquiryItemId==cvref.EnquiryItemId).Select(x => new {
                                x.SalaryExpectCurrency, x.SalaryExpectation}).FirstOrDefaultAsync();
                        msg+="<td>" + jobcard.SalaryExpectCurrency + " " + jobcard.SalaryExpectation + "</td>";
                    }
                    msg+="<td></td>";
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

        private string GetPhotoUrl (int candidateId)
        {
            var photo = _context.Attachments.Where(x => x.CandidateId == candidateId && 
                x.AttachmentType == "photo").Select(x => x.AttachmentUrl).FirstOrDefault();
            if (string.IsNullOrEmpty(photo)) return "";
            return photo;
        }

        private string GetSalaryExpectation(int candidateId, int enquiryItemId)
        {
            var sal = _context.JobCards.Where(x => x.EnquiryItemId==enquiryItemId && 
                x.CandidateId == candidateId)
                .Select(x => new {x.SalaryExpectCurrency, x.SalaryExpectation})
                .FirstOrDefault();
            if (string.IsNullOrEmpty(sal.SalaryExpectCurrency)) {
                return string.Empty;
            }else{
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
            var grd = _context.Assessments.Where(x => x.EnquiryItemId==enquiryItemId && 
                x.CandidateId == candidateId).Select(x => x.Grade).FirstOrDefault();
            if (grd==0) return "not assessed";
            return grd.ToString();
        }

        public Task<CVForward> ForwardCVsToClient(CVForward cvForward)
        {
            throw new NotImplementedException();
        }
    }
}
