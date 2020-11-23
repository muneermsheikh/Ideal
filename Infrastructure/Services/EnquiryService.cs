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
        private const int Default_ProjManagerId = 2;
        private const string Default_Ecnr ="ecr";
        private const string Default_AssessmentReqd = "f";
        private const string Default_EvalReqd = "f";
        private const int Default_ContractPeriod = 24;
        private const string SalCurrency = "";

        public EnquiryService( IUnitOfWork unitOfWork,
                IBasketRepository basketRepo, ATSContext context,
                ICustomerService customerService, IDLService dlService,
                ICategoryService catService)
        {
            _dlService = dlService;
            _catService = catService;
            _customerService = customerService;
            _context = context;
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
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
            var enquiry = new Enquiry(customerId, enquiryNo.ToString(), intDefaultProjectManagerId, 
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
            if (enq.AccountExecutiveId == 0) {
                if (offs != null || offs.Count !=0) {
                    var accountsExecId = offs.Where(x => x.Scope.ToLower() == "accounts").Select(x => x.Id).FirstOrDefault();
                    enq.AccountExecutiveId = accountsExecId == 0 ? offs[0].Id : accountsExecId;
                }
            }
            if (enq.HRExecutiveId == 0) {
                if (offs != null || offs.Count !=0) {
                    var hrexecId = offs.Where(x => x.Scope.ToLower() == "hr").Select(x => x.Id).FirstOrDefault();
                    enq.HRExecutiveId = hrexecId == 0 ? offs[0].Id : hrexecId;
                }
            }
            if (enq.LogisticsExecutiveId == 0) {
                if (offs!=null || offs.Count != 0) {
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
                if (String.IsNullOrEmpty(item.Ecnr)) item.Ecnr = Default_Ecnr;
                if (String.IsNullOrEmpty(item.AssessmentReqd)) item.AssessmentReqd = Default_AssessmentReqd;
                if (String.IsNullOrEmpty(item.EvaluationReqd)) item.EvaluationReqd = Default_AssessmentReqd;
                if (String.IsNullOrEmpty(item.ReviewStatus)) item.ReviewStatus = "NotReviewed";
                if (String.IsNullOrEmpty(item.EnquiryStatus)) item.EnquiryStatus = "NotStarted";

                if (String.IsNullOrEmpty(item.JobDesc.JobDescription)) item.JobDesc.JobDescription = "Not defined";
                if (String.IsNullOrEmpty(item.JobDesc.JobProfileUrl)) item.JobDesc.JobProfileUrl = "Not defined";
                
                if (item.Remuneration.ContractPeriodInMonths == 0) item.Remuneration.ContractPeriodInMonths = Default_ContractPeriod;
                if (String.IsNullOrEmpty(item.Remuneration.SalaryCurrency)) 
                {
                    if (SalCurrency == "") {
                        var curr = await _customerService.CustomerCountryCurrency(enq.CustomerId);
                        item.Remuneration.SalaryCurrency = curr;
                    }
                }
            }

            var enqRepo = _unitOfWork.Repository<Enquiry>();
            var nextEnqNo = await enqRepo.GetNextEnquiryNo();

            enq.EnquiryNo = (nextEnqNo + 1).ToString();

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
                .Include(x=>x.EnquiryItems).SingleOrDefaultAsync();
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
            return await _unitOfWork.Repository<Enquiry>().UpdateAsync(enq);
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

        public async Task<Remuneration> UpdateRemunerationAsync(Remuneration remuneration)
        {
            return await _unitOfWork.Repository<Remuneration>().UpdateAsync(remuneration);
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
    }
}