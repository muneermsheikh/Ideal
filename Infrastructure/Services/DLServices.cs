using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Masters;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class DLServices : IDLService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<EnquiryItem> _itemRepo;
        private readonly ATSContext _context;
        private readonly IEmployeeService _empService;
        public DLServices(IUnitOfWork unitOfWork,
                          IGenericRepository<EnquiryItem> itemRepo,
                          ATSContext context,
                          IEmployeeService empService)
        {
            _itemRepo = itemRepo;
            _unitOfWork = unitOfWork;
            _context = context;
            _empService = empService;
        }

        public async Task<IReadOnlyList<EnquiryItem>> AddDLItemsAsync(List<EnquiryItem> enquiryItems)
        {
            return await _itemRepo.AddListAsync(enquiryItems);
        }

        public async Task<int> DeleteContractReviewItem(ContractReviewItem contractReviewItem)
        {
            return await _unitOfWork.Repository<ContractReviewItem>().DeleteAsync(contractReviewItem);
        }

        public async Task<bool> DeleteDLItemAsync(EnquiryItem enquiryItem)
        {
            return await _itemRepo.DeleteAsync(enquiryItem) == 0 ? false : true;
        }

        public async Task<int> DeleteJobDescAsync(JobDesc jobDescription)
        {
            return await _unitOfWork.Repository<JobDesc>().DeleteAsync(jobDescription);
        }

        public async Task<int> DeleteRemunerationAsync(Remuneration remuneration)
        {
            return await _unitOfWork.Repository<Remuneration>().DeleteAsync(remuneration);
        }

        public async Task<ContractReview> GetContractReviewAsync(int enquiryId)
        {
            // check if ContractReviewItems has any missing enquiryitems
            var missingEnqItems = await GetEnquiryItemsMissingFromReviewItem(enquiryId);
            if (missingEnqItems.Count > 0)
            {
                var rvw = await _context.ContractReviews.Where(x => x.EnquiryId == enquiryId).SingleOrDefaultAsync();    
                if (rvw == null)
                {
                    rvw = new ContractReview(false, enquiryId, 0, "NotReviewed");
                    if (missingEnqItems != null) {     // no items were added in CreateReviewItemsOfEnquiry above.
                        var lst = new List<ContractReviewItem>();
                        foreach(var item in missingEnqItems)
                        {
                            string cat = await _context.Categories.Where(x => x.Id == item.CategoryItemId)
                            .Select(x => x.Name).SingleOrDefaultAsync();
                            lst.Add(new ContractReviewItem (item.Id, item.EnquiryId, item.Quantity, 
                                item.SrNo + "-" + cat));
                        }
                        rvw.ContractReviewItems = lst;
                        await _unitOfWork.Repository<ContractReview>().AddAsync(rvw);
                    }
                } 
            }
            
            var rvwToReturn = await _context.ContractReviews.Where(x => x.EnquiryId == enquiryId)
                .Include(x => x.ContractReviewItems).SingleOrDefaultAsync();
            return rvwToReturn;
        }

        public async Task<Enquiry> GetDLByIdAsync(int enquiryId)
        {
            return await _unitOfWork.Repository<Enquiry>().GetByIdAsync(enquiryId);
        }

        public async Task<Customer> GetDLCustomer(int enquiryId)
        {
            var custId = await _context.Enquiries.Where(x => x.Id == enquiryId).Select(x => x.CustomerId).SingleOrDefaultAsync();
            if (custId == 0) return null;
            return await _unitOfWork.Repository<Customer>().GetByIdAsync(custId);
        }

        public async Task<EnquiryItem> GetDLItemAsync(int enquiryItemId)
        {
            return await _unitOfWork.Repository<EnquiryItem>().GetByIdAsync(enquiryItemId);
        }

        public async Task<IReadOnlyList<EnquiryItem>> GetDLItemsAsync(int enquiryId, string itemStatus)
        {
            return await _itemRepo.ListWithSpecAsync(new EnquiryItemsSpecs(enquiryId, itemStatus));
        }

        public async Task<Category> GetEnquiryItemCategory(int enquiryItemId)
        {
            var item = await _itemRepo.GetByIdAsync(enquiryItemId);
            if (item == null) return null;
            return await _unitOfWork.Repository<Category>().GetByIdAsync(item.CategoryItemId);
        }

        public async Task<IReadOnlyList<Enquiry>> GetEnquiryList500WithAllStatus()
        {
            var enqs = await _context.Enquiries
                .Include(x=>x.EnquiryItems)
                .OrderByDescending(x=>x.EnquiryNo)
                .Take(500)
            .ToListAsync();
            return enqs;
        }

        public async Task<JobDesc> GetJobDescOfAnItemAsync(int EnquiryItemId)
        {
            var jd = await _unitOfWork.Repository<JobDesc>()
                .GetEntityWithSpec(new JobDescSpec(EnquiryItemId));

            if (jd == null)
            {
                var enq = await _unitOfWork.Repository<EnquiryItem>().GetByIdAsync(EnquiryItemId);
                if (enq == null) return null;
                jd = new JobDesc(EnquiryItemId, enq.EnquiryId);
                jd = await _unitOfWork.Repository<JobDesc>().AddAsync(jd);
            }
            return jd;
        }

        public async Task<IReadOnlyList<JobDesc>> GetJobDescOfEnquiryIdAsync(int enquiryId)
        {
            return await _unitOfWork.Repository<JobDesc>().GetEntityListWithSpec(
                new JobDescSpec("", enquiryId));
        }

        public async Task<ContractReviewItem> GetOrAddReviewItemAsync(int enquiryItemId)
        {
            var ReviewItem = await _context.ContractReviewItems.Where(x => x.EnquiryItemId==enquiryItemId)
                .FirstOrDefaultAsync();
            if (ReviewItem == null)
            {
                var enq = await _context.EnquiryItems.Where(x => x.Id == enquiryItemId).SingleOrDefaultAsync();

                var cRvw = await _context.ContractReviews.Where(x => x.EnquiryId == enq.EnquiryId).SingleOrDefaultAsync();
                
                if (cRvw == null) { return null;}

                string categoryName = await _context.Categories.Where(x => x.Id == enq.CategoryItemId).Select(x => x.Name).SingleOrDefaultAsync();

                await _unitOfWork.Repository<ContractReviewItem>()
                    .AddAsync(new ContractReviewItem(enquiryItemId, enq.EnquiryId, cRvw.Id, 
                        enq.Quantity, enq.SrNo + "-" + categoryName));
                ReviewItem = await _context.ContractReviewItems.Where(x => x.EnquiryItemId == enquiryItemId).SingleOrDefaultAsync();
            }

            return ReviewItem;
        }

        public async Task<Remuneration> GetRemunerationAsync(int enquiryItemId)
        {
            var remun = await _unitOfWork.Repository<Remuneration>()
                .GetEntityWithSpec(new RemunerationSpecs(enquiryItemId));

            if (remun == null)
            {
                var enq = await _unitOfWork.Repository<EnquiryItem>().GetByIdAsync(enquiryItemId);
                if (enq == null) return null;
                remun = new Remuneration(enquiryItemId, enq.EnquiryId);
                remun = await _unitOfWork.Repository<Remuneration>().AddAsync(remun);
            }

            return remun;
        }

        public async Task<IReadOnlyList<Remuneration>> GetRemunerationOfEnquiry(int enquiryId)
        {
            var spec = new RemunerationSpecs("", enquiryId);
            return await _unitOfWork.Repository<Remuneration>().GetEntityListWithSpec(spec);
        }

        public async Task<Enquiry> UpdateDLAsync(Enquiry enquiry)
        {
            return await _unitOfWork.Repository<Enquiry>().UpdateAsync(enquiry);
        }

        public async Task<EnquiryItem> UpdateDLItemAsync(EnquiryItem enquiryItem)
        {
            var cat = await _context.Categories.Where(x => x.Id == enquiryItem.CategoryItemId)
                .Select(x => x.Name).SingleOrDefaultAsync();
            
            if (cat == null) return null;
            enquiryItem.CategoryName = cat;

            if (enquiryItem.HRExecutiveId != 0)
            {
                var Nm = _empService.GetEmployeeByIdAsync((int)enquiryItem.HRExecutiveId);
                if (Nm == null) return null;
            }
            if (enquiryItem.AssessingSupId != 0)
            {
                var Nm = _empService.GetEmployeeByIdAsync((int)enquiryItem.AssessingSupId);
                if (Nm == null) return null;
            }
            if (enquiryItem.AssessingHRMId != 0)
            {
                var Nm = _empService.GetEmployeeByIdAsync((int)enquiryItem.AssessingHRMId);
                if (Nm == null) return null;
            }

            return await _itemRepo.UpdateAsync(enquiryItem);
        }

        public async Task<JobDesc> UpdateJobDescAsync(JobDesc jobDesc)
        {
            return await _unitOfWork.Repository<JobDesc>().UpdateAsync(jobDesc);
        }

        public async Task<Remuneration> UpdateRemunerationAsync(Remuneration remuneration)
        {
            return await _unitOfWork.Repository<Remuneration>().UpdateAsync(remuneration);
        }

        public async Task<ContractReview> UpdateReviewAsync(ContractReview review)
        {
            var rvw = await _unitOfWork.Repository<ContractReview>().UpdateAsync(review);
            await _unitOfWork.Repository<ContractReviewItem>().UpdateListAsync(review.ContractReviewItems);
            var rowsAffected = await UpdateReviewStatusInEnqItemsEnqCReviewAsync(review.ContractReviewItems);
            return rvw;
        }

        //updates EnquiryItem.ReviewStatus, and 
        //Enquiry.ReviewStatus, and
        //ContractReview.ReviewStatus 
        //fields based upon values in all ContractReviewItems status fields
        public async Task<int> UpdateReviewStatusInEnqItemsEnqCReviewAsync(List<ContractReviewItem> reviewItems)
        {
            var isDistinct = (from c in reviewItems select c.EnquiryId).ToList().Distinct();
            if (isDistinct.Count() > 1) throw new Exception ("Please include review items of " +
                "one DL at a time");
            var distinctIdList = isDistinct.ToArray();
            int userId=reviewItems[0].ReviewedBy;
            var bSelected=false;
            var bRejected=false;
            var items = new List<EnquiryItem>();   // update the EnquiryItem.ReviewStatus fields
            string newReviewStatus="NotReviewed";      
            foreach(var item in reviewItems)
            {
                var eItem = await _context.EnquiryItems.Where(x => x.Id == item.EnquiryItemId)
                    .FirstOrDefaultAsync();
                if (eItem.ReviewStatus != item.Status)
                {
                    eItem.ReviewStatus = item.Status;
                    items.Add(eItem);
                }

                switch(eItem.ReviewStatus)
                {
                    case "Accepted":
                        bSelected=true;
                        break;
                    case "Rejected_CustomerLowStanding":
                    case "Rejected_RequirementSuspect":
                    case "Rejected_SalaryOfferedNotFeasible":
                    case "Rejected_TechNotFeasible":
                    case "Rejected_VisasNotAvailable":
                    case "RequirementConcluded":
                        bRejected=true;
                        break;
                    default:
                        break;
                }
                if(bSelected && !bRejected) 
                {newReviewStatus = "Accepted_WithExceptions"; }
                else if(bSelected) {newReviewStatus = "Accepted";} 
                else if (bRejected) {newReviewStatus = "Declined"; }
            }
            
            var enq= await _context.Enquiries.Where(x=>x.Id == distinctIdList[0]).FirstOrDefaultAsync();
            var EnqReviewStatus_Current = enq.ReviewStatus;
            if (EnqReviewStatus_Current != newReviewStatus)
            {
                enq.ReviewedById=userId;
                enq.ReviewedOn=DateTime.Now;
                enq.ReviewStatus = newReviewStatus;
                await _unitOfWork.Repository<Enquiry>().UpdateAsync(enq);
            }
            
            if (items.Count > 0) {await _itemRepo.UpdateListAsync(items);}     //items are enquiry items with updated with ReviewStatus fields

            return await _unitOfWork.Repository<ContractReviewItem>().UpdateListAsync(reviewItems);
        }

        // inserts contractReviewItems if they don't exist, else updates the existing record
        // also calls UpdateReviewStatusInEnqItemsEnqReviewAsync
        public async Task<IReadOnlyList<ContractReviewItem>> UpdateReviewItemsAsync(IReadOnlyList<ContractReviewItem> contractReviewItems)
        {
            var rvwItemsToReturn = new List<ContractReviewItem>();   
            /*
            foreach (var item in contractReviewItems)
            {
                // check if reviewid, enquiryitemId and enquiryId o not mismatch
                var rvw = await _context.ContractReviewItems.Where(x => x.Id== item.Id && 
                    x.EnquiryItemId == item.EnquiryItemId && x.EnquiryId ==item.EnquiryId)
                    .FirstOrDefaultAsync();
                if (rvw == null) return null;   //Id, enquriyItemId, enquiryId do not match;
                // check if reviewedbyId value exists
                var emp = await _context.Employees
                    .Where(x => x.Id == item.ReviewedBy && x.IsInEmployment).FirstOrDefaultAsync();
                if (emp==null) return null;     //reviewedBy illegal
                
                if (!await EnquiryItemIdMatchesWithEnquiryId(item.Id, item.EnquiryId)) return null;
            }
            */

            var _repoReview = _unitOfWork.Repository<ContractReviewItem>();
            foreach (var item in contractReviewItems)
            {
                var rvwExists = await _repoReview.GetEntityWithSpec(new ContractReviewItemSpec(item.EnquiryItemId));
                if (rvwExists == null)
                {
                    rvwExists = await _repoReview.AddAsync(item);
                    if (rvwExists != null) rvwItemsToReturn.Add(rvwExists);
                }
                else
                {
                    var rvw = await _repoReview.UpdateAsync(item);
                    if (rvw != null) rvwItemsToReturn.Add(rvw);
                }
            }

            await UpdateReviewStatusInEnqItemsEnqCReviewAsync(rvwItemsToReturn);
            return rvwItemsToReturn;
        }

        private async Task<List<EnquiryItem>> GetEnquiryItemsMissingFromReviewItem(int enquiryId)
        {
             return await _context.EnquiryItems.Where(
                c => c.EnquiryId == enquiryId && 
                !_context.ContractReviewItems
                .Select(b => b.EnquiryItemId).Contains(c.Id))
                .ToListAsync();
        }

        private async Task<bool> EnquiryItemIdMatchesWithEnquiryId(int enquiryItemId, int enquiryId)
        {
            var item = await _itemRepo.GetEntityWithSpec(
                new EnquiryItemsSpecs(enquiryId, enquiryItemId, "EnqAndItemId"));
            return (item == null ? false : true);
        }

    }
}