using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Identity;
using Core.Entities.Masters;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class EnquiryService : IEnquiryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepo;
        private readonly ATSContext _context;
        private readonly ICustomerService _customerService;
        private readonly IDLService _dlService;
        private readonly ICategoryService _catService;
        private readonly ITaskService _taskService;
        private readonly IEmailService _emailService;
        private readonly IEmployeeService _empService;
        private const int Default_ProjManagerId = 2;
        private const bool Default_Ecnr = false;
        private const bool Default_AssessmentReqd = false;
        private const bool Default_EvaluationReqd = false;
        private const bool Default_EvalReqd = false;
        private const int Default_ContractPeriod = 24;
        private const string SalCurrency = "";

        public EnquiryService( IUnitOfWork unitOfWork,
                IBasketRepository basketRepo, ATSContext context,
                ICustomerService customerService, IDLService dlService,
                ICategoryService catService, ITaskService taskService, 
                IEmailService emailService, IEmployeeService empService)
        {
            _dlService = dlService;
            _catService = catService;
            _customerService = customerService;
            _context = context;
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
            _taskService = taskService;
            _emailService = emailService;
            _empService = empService;
        }

    // Foreign Keys for Enq
        //  AccountExecutiveId - optional, CustomerId, HRExecutiveId - optional, 
        // LogisticsExecutiveId - optional,ProjectManagerId - virtual, ReviewedById - virtual
    // Foreign keys for EnqItems
        // AssessingHRExecId - optional, AssessingHRMId-optional, AsessingSupId - optional
        // CVSourceId, CndidateId, EnquiryId, JobDescId1, RemunerationId1
        // Food-optional, Housing-optional, Transport-optional, CompleteBy-optional
        // JobDescId-optional, RemuneationId-optional
        public async Task<Enquiry> CreateEnquiryAsync(string basketId)
        {

            var basket = await _basketRepo.GetBasketAsync(basketId);
            if (basket == null) throw new Exception("failed to retrieve the basket");
            
            int customerId = basket.CustomerId; // appUser.Address.CustomerId; // GetCustomerIdFromLoggedInemail(buyerEmail, shippingAddress).Result;
            if (customerId == 0) throw new Exception("Customer Id not defined");
            int officialId = basket.OfficialId;
            if (officialId == 0) //get available official Id from db for the customer
            {
                var offId = await _context.CustomerOfficials
                    .Where(x => x.CustomerId==customerId && x.IsValid.ToLower()=="t")      //consider order by scope
                    .Select(x => x.Id).FirstOrDefaultAsync();
                if (offId == 0) throw new Exception("the Official Id does not correspond to the customer");
                officialId=offId;
            }
            
            var enquiryItems = new List<EnquiryItem>();
            var jdList = new List<JobDesc>();
            var remunList = new List<Remuneration>();

            int i = 1;
            // int newOrderNo = async _unitOfWork.Repository<Enquiry>.Select(p => p.Age).Cast<int?>().Max() ?? 0;
            foreach (var item in basket.Items)
            {
                // not trusting basket from the client, categoryName reset to actual value from database
                // var catItem = await _unitOfWork.Repository<Category>().GetByIdAsync(item.CategoryId);
                var catItemOrdered = new CategoryItemOrdered(item.CategoryId, item.CategorytName);

                var enqItem = new EnquiryItem(catItemOrdered, i, item.Quantity, item.Ecnr, item.CompleteBy);
                    /* item.ExpDesiredInYrsMin, item.ExpDesiredInYrsMax,
                    item.JobDescInBrief, item.JobDescAttachment, item.SalaryRangeMin,
                    item.SalaryRangeMax, item.ContractPeriodInMonths, item.Food,
                    item.Housing, item.Transport, item.CompleteBy); */

                enquiryItems.Add(enqItem);
                
                jdList.Add(new JobDesc(0, i, item.JobDescInBrief, item.JobDescAttachment));
                
                remunList.Add(new Remuneration(0, i++, item.ContractPeriodInMonths, item.SalaryCurrency,
                    item.SalaryRangeMin, item.SalaryRangeMax, item.SalaryNegotiable, item.Housing, 
                    item.HousingAllowance, item.Food, item.FoodAllowance, item.Transport, 
                    item.TransportAllowance, item.OtherAllowance,
                    item.LeaveAfterMonths, item.LeavePerYear, DateTime.Now));
            }

            var enqRepo = _unitOfWork.Repository<Enquiry>();

            int enquiryNo = await enqRepo.GetNextEnquiryNo() +1;
            var enqRef = "";
            int intDefaultProjectManagerId = 1;     //*** this shd be part of default values

            // create Enquiry
            // *** replace second hrExecutiveId with logisticsExecutiveId
            var enquiry = new Enquiry(customerId, enquiryNo, intDefaultProjectManagerId, 
                enqRef, basketId, officialId, officialId,  enquiryItems);

            var enqAdded = await enqRepo.AddAsync(enquiry);            

            if(enqAdded==null) throw new Exception("Failed to add the enquiry");
            
            // add JobDesc and Remuneration for each enquiryItem
           for(int j = 1; j < i; j++)
            {
                var enqItem = await _unitOfWork.Repository<EnquiryItem>().GetEntityWithSpec(
                    new EnquiryItemsSpecs(j, enqAdded.Id));
                if (enqItem != null)
                {
                    jdList[j-1].EnquiryItemId=enqItem.Id;
                    jdList[j-1].EnquiryId=enqAdded.Id;
                    remunList[j-1].EnquiryId=enqAdded.Id;
                    remunList[j-1].EnquiryItemId=enqItem.Id;
                    enqItem.JobDesc=jdList[j-1];
                    enqItem.Remuneration=remunList[j-1];
                }
            }
            
            await _unitOfWork.Repository<JobDesc>().AddListAsync(jdList);
            //adding JobDesc also adds Remuneration to the db
            //await _unitOfWork.Repository<Remuneration>().AddListAsync(remunList);
            
            enqAdded.EnquiryItems=(List<EnquiryItem>) await _unitOfWork.Repository<EnquiryItem>().GetEntityListWithSpec(
                new EnquiryItemsSpecs(enqAdded.Id, "NotReviewed"));
    
            // delete basket
            await _basketRepo.DeleteBasketAsync(basketId);
            // return enquiry

            return enqAdded;

        }
    
        // AddAsync is used by Agency Officials - here the basket is not relevant.
        public async Task<Enquiry> AddEnquiryAsync(Enquiry enq)
        {
         // enquiry parent   
            if (enq.CustomerId == 0) {
                throw new Exception ("Customer not defined");
            }
            var enquiryItems = enq.EnquiryItems;
            if (enquiryItems == null || enquiryItems.Count == 0) {
                throw new Exception ("No items present");
            }

            enq.BasketId = "";
            if (enq.ProjectManagerId == 0) {
                enq.ProjectManagerId = Default_ProjManagerId;
            }
            
            if (String.IsNullOrEmpty(enq.ReadyToReview)) enq.ReadyToReview = "No";
            if (String.IsNullOrEmpty(enq.ReviewStatus)) enq.ReviewStatus = "NotReviewed";
            if (enq.CompleteBy == null) enq.CompleteBy = enq.EnquiryDate.AddDays(7);   //default


        // customer officials    
            var offs = await _customerService.GetCustomerOfficialList(enq.CustomerId);
            if (offs != null || offs.Count > 0) {
                if (enq.AccountExecutiveId == null) {
                    var accountsExecId = offs.Where(x => x.Scope.ToLower() == "accounts").Select(x => x.Id).FirstOrDefault();
                    enq.AccountExecutiveId = accountsExecId == 0 ? offs[0].Id : accountsExecId;
                }
                if (enq.HRExecutiveId == null) {
                    var hrexecId = offs.Where(x => x.Scope.ToLower() == "hr").Select(x => x.Id).FirstOrDefault();
                    enq.HRExecutiveId = hrexecId == 0 ? offs[0].Id : hrexecId;
                }
                if (enq.LogisticsExecutiveId == null) {
                    var logisticsId = offs.Where(x => x.Scope.ToLower() == "logistics").Select(x => x.Id).FirstOrDefault();
                    enq.LogisticsExecutiveId = logisticsId == 0 ? offs[0].Id : logisticsId;
                }
            }

            var jdList = new List<JobDesc>();
            var remunList = new List<Remuneration>();

        // items    
            int srNo = 0;
            foreach(var item in enq.EnquiryItems) 
            {
                item.SrNo=++srNo;
                item.CategoryName = _catService.GetCategoryNameFromCategoryId(item.CategoryItemId);
                if (item.MaxCVsToSend == 0) item.MaxCVsToSend = item.Quantity * 3;
                if (item.Ecnr==null) item.Ecnr = Default_Ecnr;
                if (item.AssessmentReqd==null) item.AssessmentReqd = Default_AssessmentReqd;
                if (item.EvaluationReqd==null) item.EvaluationReqd = Default_EvaluationReqd;
                if (String.IsNullOrEmpty(item.ReviewStatus)) item.ReviewStatus = "NotReviewed";
                if (String.IsNullOrEmpty(item.EnquiryStatus)) item.EnquiryStatus = "NotStarted";

                if (item.JobDesc != null) {
                    if (String.IsNullOrEmpty(item.JobDesc.JobDescription)) item.JobDesc.JobDescription = "Not defined";
                    if (String.IsNullOrEmpty(item.JobDesc.JobProfileUrl)) item.JobDesc.JobProfileUrl = "Not defined";
                }
                if (item.Remuneration != null) {
                    if (item.Remuneration.ContractPeriodInMonths == 0) item.Remuneration.ContractPeriodInMonths = Default_ContractPeriod;
                    if (String.IsNullOrEmpty(item.Remuneration.SalaryCurrency)) 
                    {
                        if (SalCurrency == "") {
                            var curr = await _customerService.CustomerCountryCurrency(enq.CustomerId);
                            item.Remuneration.SalaryCurrency = curr;
                        }
                    }
                }
            }

            var enqRepo = _unitOfWork.Repository<Enquiry>();
            var nextEnqNo = await enqRepo.GetNextEnquiryNo();

            enq.EnquiryNo = nextEnqNo + 1;

            var enqAdded = await enqRepo.AddAsync(enq);            

            if(enqAdded==null) throw new Exception("Failed to add the enquiry");

            return enqAdded;
        }
    
        public async Task<int> DeleteEnquiryAsync(int Id)
        {
            var enq = new Enquiry{Id = Id};
            if (enq == null) return 0;
            return await _unitOfWork.Repository<Enquiry>().DeleteAsync(enq);
        }
        public async Task<IReadOnlyList<Enquiry>> GetEnquiries(EnquiryParams eParams)
        {
            var spec = new EnquirySpecs(eParams);
            var specCt = new EnquirySpecsCount(eParams);
            return await _unitOfWork.Repository<Enquiry>().GetEntityListWithSpec(spec);
        }

        public async Task<Enquiry> GetEnquiryWithSpecByIdAsync(int enquiryId)
        {
            //return await _unitOfWork.Repository<Enquiry>().GetByIdAsync(enquiryId); 
            return await _unitOfWork.Repository<Enquiry>().GetEntityWithSpec(
                new EnquirySpecs(enquiryId, "Accepted", false, false));
        }

        public async Task<Enquiry> GetEnquiryByIdAsync(int enquiryId)
        {
            
            var enq = await _context.Enquiries.Where(x=>x.Id==enquiryId)
                .Include(x => x.EnquiryItems).ThenInclude(x => x.Remuneration)
                .Include(x => x.EnquiryItems).ThenInclude(x => x.JobDesc)
                .SingleOrDefaultAsync();
            
            /*
            var enq = await _context.Enquiries.Where(x => x.Id == enquiryId)
                .ToListAsync()
                .Select( en => new {
                    Items = en.EnquiryItems.OrderBy(x => x.SrNo).ToListAsync(),
                    rem = en.Remuneration,
                    jd = en.JobDesc
                }
                ).ToListAsync();
            
            /*
            foreach(var item in enq.EnquiryItems)
            {
                if (item.Remuneration == null || item.Remuneration.Id == null)
                {
                    var rem = await _unitOfWork.Repository<Remuneration>().AddAsync(
                        new Remuneration(item.Id, item.EnquiryId));
                }
                if (item.JobDesc == null || item.JobDesc.Id == null)
                {
                    var jd = await _unitOfWork.Repository<JobDesc>().AddAsync(
                        new JobDesc(item.Id, item.EnquiryId));
                }
            }
            */

            /*
            var eq = await _context.Enquiries.Where(x => x.Id == enquiryId)
                .Select(x => new {
                    en = x,
                    it = x.EnquiryItems.OrderBy(s => s.SrNo)
                })
                .SingleOrDefaultAsync();
            var enq = eq.Select(g => g.en).FirstOrDefault();
                /*
                    var groups = await db.Parents
                    .Where(p => p.Id == id)
                    .Select(p => new
                        {
                            P = p,
                            C = p.Children.OrderBy(c => c.SortIndex)
                        })
                    .ToArrayAsync();

                    // Query/db interaction is over, now grab what we wanted from what was fetched

                    var model = groups
                    .Select(g => g.P)
                    .FirstOrDefault()
                    */
            return enq;
        }

        public async Task<IReadOnlyList<Enquiry>> GetEntityListWithSpec(EnquiryParams enqParam)
        {
            return await _unitOfWork.Repository<Enquiry>().GetEntityListWithSpec(new EnquirySpecs(enqParam));
        }

        public async Task<IReadOnlyList<Enquiry>> GetEnquiryList500WithAllStatus()
        {
            var enqs = await _context.Enquiries.Include(x => x.EnquiryItems)
                .OrderByDescending(x => x.EnquiryDate).Take(500).ToListAsync();
            return enqs;
        }

        public async Task<Enquiry> UpdateDLAsync(Enquiry enq)
        {

        // A - identify records in DB that are missing in the model
            var enqItemsInDB = await _context.EnquiryItems.Where(x => x.EnquiryId == enq.Id).AsNoTracking().ToListAsync();
            var enqItemIdsInModel = enq.EnquiryItems.Select(x => x.Id);

            // create a list to store items to delete from DB
            var enqItemsToDelete = new List<EnquiryItem>();
            
            if (enqItemIdsInModel == null) {    // find enquiryItemIds to delete from DB
                enqItemsToDelete = await _context.EnquiryItems.Where(x => x.EnquiryId == enq.Id)
                    .AsNoTracking().ToListAsync();
            } else 
            {                            // find enquiryItems from DB that are missing in the model, these are to be deleted from DB
                enqItemsToDelete = await _context.EnquiryItems.Where(x => x.EnquiryId == enq.Id && !enqItemIdsInModel
                    .Contains(x.Id)).AsNoTracking().ToListAsync();
            }
        // B - DELETE identified recoords from DB
            if (enqItemsToDelete != null || enqItemsToDelete.Count > 0)
            {
                var deleted = await _unitOfWork.Repository<EnquiryItem>().DeleteListAsync(enqItemsToDelete);
            }
        
        //C - identify items that are present both in DB and model - for updates
            var enqItemIdsToUpdate = await _context.EnquiryItems.Where(x => enqItemIdsInModel.Contains(x.Id))
                .Select(x => x.Id).ToListAsync();
            var enqItemsFromModelToUpdate = enq.EnquiryItems.Where(x => enqItemIdsToUpdate.Contains(x.Id)).ToList();
            
            // filter our records to be updated, else these records will be again INSERTED along with Enquiry entity when it is updted
            // create another entity excluding records that hv been updated as above - note the ! operator to exclude
            var enqItemModelFiltered = enq.EnquiryItems.Where(x => !enqItemIdsToUpdate.Contains(x.Id)).ToList();
            // filter out the records to be updated, else these records will be again INSERTED along with the customerEntity when it is updated
            var enqItemsToUpdate = enq.EnquiryItems.Where(x => enqItemIdsToUpdate.Contains(x.Id)).ToList();
            
            // Begin writing to DB, do the SaveAsync in the end to allow transactions    
            //await _unitOfWork.Repository<EnquiryItem>().UpdateListAsync(enqItemsToUpdate);
            foreach(var item in enqItemsToUpdate)
            {
                //_context.Set<T>().Attach(item);
                _context.EnquiryItems.Attach(item);
                _context.Entry(item).State = EntityState.Modified;
            }

            // ascertain the new entity contains the parent key of Enquiry Id
        // D - attach enqItemModelFiltered to Enquiry model

            // await _unitOfWork.Repository<EnquiryItem>().AddListAsync(enqItemModelFiltered);
            foreach(var item in enqItemModelFiltered)
            {
                //_context.Set<T>().Attach(item);
                _context.EnquiryItems.Attach(item);
                _context.Entry(item).State = EntityState.Modified;
            }
        // E - all items having been deleted, updated or inserted, set the EnquiryItems to null, else those will be inserted again
        //    enq.EnquiryItems = null;  this has to wait till JD and remuneration items are updated

        // JobDesc and Remuneration are one-to-one relationships, so there is no delete or insert, just updates.
            var jdList = new List<JobDesc>();
            var remunList = new List<Remuneration>();
            foreach (var item in enq.EnquiryItems)
            {
                var jd = item.JobDesc;
                if (jd != null)
                {
                    jdList.Add(jd);
                }

                var remun = item.Remuneration;
                if (remun != null)
                {
                    remunList.Add(remun);
                }
            } 

            if (jdList != null || jdList.Count > 0) 
            {
                 foreach(var item in jdList)
                {
                    _context.JobDescriptions.Attach(item);
                    _context.Entry(item).State = EntityState.Modified;
                }
            }

            if (remunList != null || remunList.Count > 0) 
            {
                 foreach(var item in remunList)
                {
                    _context.Remunerations.Attach(item);
                    _context.Entry(item).State = EntityState.Modified;
                }
            }

        // F - Create HR Assignment Tasks
        
            var ItemsForTasks = new List<int>();
            foreach(var item in enq.EnquiryItems)
            {
                if (item.HRExecutiveId != 0)
                {
                    var t = await GetTask("HRAssignment", "active", item.Id);
                    if (t == null)  //create new task
                    {
                        ItemsForTasks.Add(item.Id);
                        var tsk = new ToDo(enq.ProjectManagerId, (int)item.HRExecutiveId, DateTime.Now, 
                            DateTime.Now.AddDays(7),  "Submit compliant CVs for category ref: " +
                            enq.EnquiryNo + "-" + item.SrNo + "-" + 
                            _catService.GetCategoryNameFromCategoryId(item.CategoryItemId), 
                            "HRExecAssignment", item.EnquiryId, item.Id);
                    } else if (t.TaskItems == null) {
                        ItemsForTasks.Add(item.Id);
                        if (t.AssignedToId != item.HRExecutiveId)      //HR Exec changed, 
                            // cancel existing task and create new task
                        {
                            await _taskService.UpdateTaskStatus(t.Id, "task reassigned to " +
                                _empService.GetEmployeeName((int)item.HRExecutiveId), DateTime.Now, 
                                t.TaskDescription + ", canceled");
                            t.AssignedToId = (int)item.HRExecutiveId;
                            t.TaskDate = DateTime.Now;
                            t.OwnerId = enq.ProjectManagerId;
                            await _taskService.UpdateTaskStatus(t.Id, "canceled", DateTime.Now, 
                                "task assigned to " + _empService.GetEmployeeName((int)item.HRExecutiveId));
                        }
                    }
                }
            }
            // enq.EnquiryItems = null;
            // finally update the parent entity Enquiry
            //return await _unitOfWork.Repository<Enquiry>().UpdateAsync(enq);
            _context.Enquiries.Attach(enq);
            _context.Entry(enq).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // send email to HR Executives
            if (ItemsForTasks.Count > 0) {
                var mailssent = await _emailService.ComposeHRTaskAssignmentMessageBody(ItemsForTasks, 
                    enq.EnquiryNo, enq.EnquiryDate, enq.CustomerId, enq.ProjectManagerId);
            }

            return enq;            

        }

        private async Task<ToDo> GetTask (string taskType, 
            string taskStatus, int enquiryItemId)
        {
            var qry = await _context.ToDos
                .Where(x => x.TaskType.ToLower() == taskType.ToLower() &&
                    x.TaskStatus.ToLower() != "completed" &&
                    x.EnquiryItemId == enquiryItemId).Include(x => x.TaskItems)
                .SingleOrDefaultAsync();
            return qry;
        }

 
        public async Task<int> GetEnquiryItemsCountNotReviewed(int enquiryId)
        {
            var spec = new EnquiryItemsSpecs(enquiryId, (int)enumItemReviewStatus.NotReviewed);
            return await _unitOfWork.Repository<EnquiryItem>().CountWithSpecAsync(spec);
        }

        public async Task<Enquiry> GetEnquiryByEnquiryItemIdAsync(int enquiryItemId)
        {
            var item =  await _unitOfWork.Repository<EnquiryItem>().GetByIdAsync(enquiryItemId);
            if (item==null) return null;
            return await _unitOfWork.Repository<Enquiry>().GetByIdAsync(item.EnquiryId);
        }
        public async Task<EnquiryItem> GetEnquiryItemByIdAsync(int enquiryItemId)
        {
            return await _unitOfWork.Repository<EnquiryItem>().GetByIdAsync(enquiryItemId); 
        }

        public async Task<IReadOnlyList<Enquiry>> GetUserEnquiriesAsync(int CustomerId)
        {
            return await _unitOfWork.Repository<Enquiry>().GetEntityListWithSpec(
                new EnquirySpecs(CustomerId, ""));
        }

        public async Task<bool> UpdateEnquiryReadyToReview(Enquiry enq)
        {
            enq.ReadyToReview = "t";

            var x = await _unitOfWork.Repository<Enquiry>().UpdateAsync(enq);

            if (x == null) return false;

            // create task for the Project Manager
            var d= enq.ReviewedOn.HasValue ? enq.ReviewedOn.Value.Date : DateTime.Now.Date;

            var todo = new ToDo(
                enq.ProjectManager.Id, enq.ProjectManager.Id, d, d.AddDays(4),
                "Following task assigned to you",
                "HRDeptHeadAssignment", enq.Id, 0
            );

            var todoAdded = await _unitOfWork.Repository<ToDo>().AddAsync(todo);
            // *** this will also trigger email message from database prvider   
            return (todoAdded != null);
        }

// employees
        public async Task<Employee> EmployeeToReturn(int employeeId)
        {
            return await _unitOfWork.Repository<Employee>().GetByIdAsync(employeeId);
        }

// job desc
        public async Task<JobDesc> GetJobDescriptionBySpecAsync(int enquiryItemId)
        {
            var jd = await _unitOfWork.Repository<JobDesc>().GetEntityWithSpec(
                new JobDescSpec(enquiryItemId));

            if (jd == null)
            {
                var enqItem = await _unitOfWork.Repository<EnquiryItem>().GetByIdAsync(enquiryItemId);
                int enquiryId = enqItem.EnquiryId;
                var jdesc = new JobDesc(enquiryItemId, enquiryId);
                jd = await _unitOfWork.Repository<JobDesc>().AddAsync(jdesc);
            }
            return jd;
        }

        public async Task<JobDesc> UpdateJDAsync(JobDesc jobDesc)
        {
            return await _unitOfWork.Repository<JobDesc>().UpdateAsync(jobDesc);
        }


// remuneration
        public async Task<Remuneration> GetRemunerationBySpecEnquiryItemIdAsync(int enquiryItemId)
        {
            var spec = new RemunerationSpecs(enquiryItemId);
            var remun = await _unitOfWork.Repository<Remuneration>().GetEntityWithSpec(spec);

            if (remun == null)
            {
               var enqItem = await _unitOfWork.Repository<EnquiryItem>().GetByIdAsync(enquiryItemId);
                if (enqItem == null) return null;
                int enquiryId = enqItem.EnquiryId;
                var remn = new Remuneration(enquiryItemId, enquiryId);
                remun = await _unitOfWork.Repository<Remuneration>().AddAsync(remn);
            }
            return remun;
        }

        public async Task<IReadOnlyList<Remuneration>> GetRemunerationsEnquiryAsync(int enquiryId)
        {
            var missingIds = await GetMissingRemunerationFromEnquiryId(enquiryId);
            if (missingIds != null)
            {
                var remList = new List<Remuneration>();
                foreach(var item in missingIds)
                {
                    if (item != 0 ) remList.Add(new Remuneration(item, enquiryId));
                }
                if (remList.Count > 0) {
                    var remAdded = await _unitOfWork.Repository<Remuneration>().AddListAsync(remList);
                }
            }

            var rems =  await _context.Remunerations.Where(x => x.EnquiryId == enquiryId).OrderBy(x => x.EnquiryItemId)
                .ToListAsync();
            // update categoryName
            foreach(var item in rems)
            {
                var stName = _catService.GetCategoryNameWithRefFromEnquiryItemId(item.EnquiryItemId);
                item.CategoryName = stName;
            }

            return rems;
        }


        public async Task<Remuneration> UpdateRemunerationAsync(Remuneration remuneration)
        {
            return await _unitOfWork.Repository<Remuneration>().UpdateAsync(remuneration);
        }

        public async Task<int> UpdateRemunerationsAsync(List<Remuneration> remuns)
        {
            
            return await _unitOfWork.Repository<Remuneration>().UpdateListAsync(remuns);
        }

    //PRIVATE METHODS
        // if a customer with email exists, get the entity and return its Id
        // else, insert the entity and gets its Id
        private async Task<int> GetCustomerIdFromLoggedInemail(string email, SiteAddress siteAddress)
        {
            var cust = await _unitOfWork.Repository<Customer>().GetCustomerFromEmailAsync(email);

            if (cust == null)
            {
                // insert customer
                string sCustomerName = siteAddress.FirstName;
                string sCity = siteAddress.City;
                string sMobile = "";
                string sIntroducedBy ="";
                
                var add = new CustomerAddress {
                    Address1 = siteAddress.Street,
                    City = siteAddress.City,
                    PIN = siteAddress.Zipcode,
                    State = siteAddress.State,
                    AddressType = Enum.GetName(typeof(enumCustomerType),enumCustomerType.Customer)
                };

                cust = new Customer("Customer", sCustomerName, 
                    sCustomerName, sCity, email, sMobile, sIntroducedBy);   //, add);

                await _unitOfWork.Repository<Customer>().AddAsync(cust);
            }
            return cust.Id;
        }
        private async Task<bool> EnquiryItemIdMatchesWithEnquiryId(int enquiryItemId, int enquiryId)
        {
            var item = await _unitOfWork.Repository<EnquiryItem>().GetEntityWithSpec(
                new EnquiryItemsSpecs(enquiryId, enquiryItemId, "EnqAndItemId"));
            return (item == null ? true: false);
        }

        public async Task<CategoryRefFromEnquiryItemId> GetDetailsFromEnquiryItemId(int enquiryItemId)
        {   
           
           var rs = await
             (from o in _context.Enquiries join e in _context.EnquiryItems
                on o.Id equals e.EnquiryId join c in _context.Categories 
                on e.CategoryItemId equals c.Id 
                where e.Id == enquiryItemId
                select (new { orderNo = o.EnquiryNo, orderid=o.Id, orderDt = o.EnquiryDate, 
                    srNo = e.SrNo, catName=c.Name, customerName = o.Customer.CustomerName, 
                    cityName=o.Customer.City })
            ).FirstOrDefaultAsync();
           
            var cat = new CategoryRefFromEnquiryItemId(rs.customerName, rs.cityName, rs.orderid,
                rs.orderNo + "-" + rs.srNo + "-" + rs.catName, rs.orderNo + "/" + rs.orderDt);
            return cat;
        }

        private async Task<List<int>> GetMissingRemunerationFromEnquiryId(int enquiryId)
        {
             /*
             var lst = await _context.EnquiryItems.Where(
                c => c.EnquiryId == enquiryId && 
                !_context.Remunerations
                .Select(b => b.EnquiryItemId).Contains(c.Id))
                .ToListAsync()
                
            if (lst == null || lst.Count == 0)
            {
                return null;
            } else {
                return lst;
            }

        var ans = from h in header
          join d1 in detail1 on h.id equals d1.id into hd1j
          from hd1 in hd1j.DefaultIfEmpty()
          from d2 in detail2 where h.id == d2.id && (hd1?.date == null || hd1.date == d2?.date)
          select new { h.id, h.label, date = hd1?.date ?? d2?.date, value1 = hd1?.value, value2 = d2?.value };
        */
            var ans = await (from ei in _context.EnquiryItems where ei.EnquiryId == enquiryId 
                join rm in _context.Remunerations on ei.Id equals rm.EnquiryItemId into eirmj 
                from eirm in eirmj.DefaultIfEmpty() 
                select eirm.EnquiryItemId)
                .ToListAsync();
            
            return ans;
        }

    }
}