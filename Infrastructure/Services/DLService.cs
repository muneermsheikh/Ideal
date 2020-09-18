using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Masters;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class DLService : IDLService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<EnquiryItem> _itemRepo;
        private readonly IEmployeeService _empService;
        private readonly ATSContext _context;
        public DLService(IUnitOfWork unitOfWork, IGenericRepository<EnquiryItem> itemRepo, ATSContext context,
            IEmployeeService empService)
        {
            _context = context;
            _empService = empService;
            _itemRepo = itemRepo;
            _unitOfWork = unitOfWork;
        }

        //DL
        public async Task<Enquiry> GetDLByIdAsync(int enquiryId)
        {
            return await _unitOfWork.Repository<Enquiry>().GetByIdAsync(enquiryId);
        }

        public async Task<IReadOnlyList<Enquiry>> GetDLIndexLast500Async()
        {
            //return await _unitOfWork.Repository<Enquiry>().ListAllAsync();
            return await _context.Enquiries
                .Include(x=>x.EnquiryItems)
                .OrderBy(x=>x.EnquiryNo)
                .Take(500)
            .ToListAsync();
        }

        public async Task<Enquiry> UpdateDLAsync(Enquiry enquiry)
        {
            return await _unitOfWork.Repository<Enquiry>().UpdateAsync(enquiry);
        }

        public async Task<Customer> GetDLCustomer(int enquiryId)
        {
            var enq = await _unitOfWork.Repository<Enquiry>().GetByIdAsync(enquiryId);
            if (enq == null) return null;
            return await _unitOfWork.Repository<Customer>().GetByIdAsync(enq.CustomerId);
        }

//DL Items
        public async Task<EnquiryItem> GetDLItemAsync(int enquiryItemId)
        {
            return await _unitOfWork.Repository<EnquiryItem>().GetByIdAsync(enquiryItemId);
        }

        public async Task<IReadOnlyList<EnquiryItem>> GetDLItemsAsync(int enquiryId,
            enumItemReviewStatus itemStatus)
        {
            //var spec = new EnquiryItemsSpecs(enquiryId, itemStatus);
            //return await _unitOfWork.Repository<EnquiryItem>().ListWithSpecAsync(spec);
            return await _itemRepo.ListWithSpecAsync(new EnquiryItemsSpecs(enquiryId, itemStatus));
        }

        public async Task<EnquiryItem> UpdateDLItemAsync(EnquiryItem enquiryItem)
        {
            var cat = await _unitOfWork.Repository<Category>().GetByIdAsync(enquiryItem.CategoryItemId);
            if (cat == null) return null;
            enquiryItem.CategoryName = cat.Name;

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

        public async Task<IReadOnlyList<EnquiryItem>> AddDLItemsAsync(List<EnquiryItem> enquiryItems)
        {
            return await _itemRepo.AddListAsync(enquiryItems);
        }

        public async Task<bool> DeleteDLItemAsync(EnquiryItem enquiryItem)
        {
            return await _itemRepo.DeleteAsync(enquiryItem) == 0 ? false : true;
        }

        public async Task<Category> GetEnquiryItemCategory(int enquiryItemId)
        {
            var item = await _itemRepo.GetByIdAsync(enquiryItemId);
            if (item == null) return null;
            return await _unitOfWork.Repository<Category>().GetByIdAsync(item.CategoryItemId);
        }

// CONRACT REVIEW ITEMS
        public async Task<int> DeleteContractReviewItem(ContractReviewItem contractReviewItem)
        {
            return await _unitOfWork.Repository<ContractReviewItem>().DeleteAsync(contractReviewItem);
        }

        //adds missing contractreviewitem records in an Enquiry
        public async Task<IReadOnlyList<ContractReviewItem>> GenerateReviewItemsOfAnEnquiryAsync(
            int enquiryId)
        {
            return await CreateReviewItemsOfEnquiry(enquiryId);
        }

        //adds a contract review item if one does not exist
        public async Task<ContractReviewItem> GetOrAddReviewItemAsync(int enquiryItemId)
        {
            //var item = await _unitOfWork.Repository<ContractReviewItem>()
                //.GetEntityWithSpec(new ContractReviewItemSpec(enquiryItemId));
            var item = await _context.ContractReviewItems.Where(x => x.EnquiryItemId==enquiryItemId)
                .FirstOrDefaultAsync();
            if (item == null)
            {
                var enq = await _unitOfWork.Repository<EnquiryItem>().GetByIdAsync(enquiryItemId);
                var newItem = new ContractReviewItem(enquiryItemId, enq.EnquiryId);
                item = await _unitOfWork.Repository<ContractReviewItem>().AddAsync(newItem);
            }

            return item;
        }

        public async Task<IReadOnlyList<ContractReviewItem>> GetOrAddReviewItemsOfEnquiryAsync(int enquiryId)
        {
            var rvws = await CreateReviewItemsOfEnquiry(enquiryId); 
            return rvws;
        }

       public async Task<ContractReviewItem> UpdateReviewItemAsync(ContractReviewItem reviewItem)
        {
            return  await _unitOfWork.Repository<ContractReviewItem>().UpdateAsync(reviewItem);

        }

        public async Task<int> UpdateReviewItemListAsync(List<ContractReviewItem> reviewItems)
        {
            var isDistinct = (from c in reviewItems select c.EnquiryId).ToList().Distinct();
            if (isDistinct.Count() > 1) throw new Exception ("Please include review items of " +
                "one DL at a time");
            var distinctIdList = isDistinct.ToArray();
            int userId=reviewItems[0].ReviewedBy;
            var bSelected=false;
            var bRejected=false;
            var items = new List<EnquiryItem>();
            foreach(var item in reviewItems)
            {
                var eItem = await _context.EnquiryItems.Where(x => x.Id == item.EnquiryItemId)
                    .FirstOrDefaultAsync();
                eItem.Status = item.Status;
                items.Add(eItem);
                switch(eItem.Status)
                {
                    case enumItemReviewStatus.Accepted:
                        bSelected=true;
                        break;
                    case enumItemReviewStatus.Rejected_CustomerLowStanding:
                    case enumItemReviewStatus.Rejected_RequirementSuspect:
                    case enumItemReviewStatus.Rejected_SalaryOfferedNotFeasible:
                    case enumItemReviewStatus.Rejected_TechNotFeasible:
                    case enumItemReviewStatus.Rejected_VisasNotAvailable:
                    case enumItemReviewStatus.RequirementConcluded:
                        bRejected=true;
                        break;
                    default:
                        break;
                }
                var enq= await _context.Enquiries.Where(x=>x.Id == distinctIdList[0]).FirstOrDefaultAsync();
                
                enq.ReviewedById=userId;
                enq.ReviewedOn=DateTime.Now;

                if(bSelected && !bRejected) 
                {enq.EnquiryReviewStatusId = enumEnquiryReviewStatus.Accepted_WithExceptions; }
                else if(bSelected) {enq.EnquiryReviewStatusId = enumEnquiryReviewStatus.Accepted;} 
                else if (bRejected) {enq.EnquiryReviewStatusId = enumEnquiryReviewStatus.Declined; }
                
                await _unitOfWork.Repository<Enquiry>().UpdateAsync(enq);
            }
            
            await _itemRepo.UpdateListAsync(items);

            return await _unitOfWork.Repository<ContractReviewItem>().UpdateListAsync(reviewItems);
        }

        public async Task<IReadOnlyList<ContractReviewItem>> UpdateReviewItemsAsync(
            IReadOnlyList<ContractReviewItem> contractReviewItems)
        {
            var items = new List<ContractReviewItem>();
            //var enqId = contractReviewItems[0].EnquiryId;

            foreach (var item in contractReviewItems)
            {
                // check if reviewid, enquiryitemId and enquiryId do not mismatch
                var rvw = await _context.ContractReviewItems.Where(x => x.Id== item.Id && 
                    x.EnquiryItemId == item.EnquiryItemId && x.EnquiryId ==item.EnquiryId)
                    .FirstOrDefaultAsync();
                if (rvw == null) return null;   //Id, enquriyItemId, enquiryId do not match;
                //check if reviewedbyId value exists
                var emp = await _context.Employees
                    .Where(x => x.Id == item.ReviewedBy && x.IsInEmployment).FirstOrDefaultAsync();
                if (emp==null) return null;     //reviewedBy illegal
                
                if (!await EnquiryItemIdMatchesWithEnquiryId(item.Id, item.EnquiryId)) return null;
            }

            var _repoReview = _unitOfWork.Repository<ContractReviewItem>();
            foreach (var item in contractReviewItems)
            {
                var rvwExists = await _repoReview.GetEntityWithSpec(new ContractReviewItemSpec(item.EnquiryItemId));
                if (rvwExists == null)
                {
                    rvwExists = await _repoReview.AddAsync(item);
                    if (rvwExists != null) items.Add(rvwExists);
                }
                else
                {
                    var rvw = await _repoReview.UpdateAsync(item);
                    if (rvw != null) items.Add(rvw);
                }
            }
            return items;
        }



    //JOB DESC        
        public async Task<int> DeleteJobDescAsync(JobDesc jobDescription)
        {
            return await _unitOfWork.Repository<JobDesc>().DeleteAsync(jobDescription);
        }

        public async Task<JobDesc> GetJobDescOfAnItemAsync(int enquiryItemId)
        {
            var jd = await _unitOfWork.Repository<JobDesc>()
                .GetEntityWithSpec(new JobDescSpec(enquiryItemId));

            if (jd == null)
            {
                var enq = await _unitOfWork.Repository<EnquiryItem>().GetByIdAsync(enquiryItemId);
                if (enq == null) return null;
                jd = new JobDesc(enquiryItemId, enq.EnquiryId);
                jd = await _unitOfWork.Repository<JobDesc>().AddAsync(jd);
            }

            return jd;
        }

        public async Task<IReadOnlyList<JobDesc>> GetJobDescOfEnquiryIdAsync(int enquiryId)
        {
            return await _unitOfWork.Repository<JobDesc>().GetEntityListWithSpec(
                new JobDescSpec("", enquiryId));
        }

        public async Task<JobDesc> UpdateJobDescAsync(JobDesc jobDesc)
        {
            return await _unitOfWork.Repository<JobDesc>().UpdateAsync(jobDesc);
        }

//REMUNERATIONS
        public async Task<int> DeleteRemunerationAsync(Remuneration remuneration)
        {
            return await _unitOfWork.Repository<Remuneration>().DeleteAsync(remuneration);
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

        public async Task<Remuneration> UpdateRemunerationAsync(Remuneration remuneration)
        {
            return await _unitOfWork.Repository<Remuneration>().UpdateAsync(remuneration);
        }
//privates
        private async Task<bool> EnquiryItemIdMatchesWithEnquiryId(int enquiryItemId, int enquiryId)
        {
            var item = await _itemRepo.GetEntityWithSpec(
                new EnquiryItemsSpecs(enquiryId, enquiryItemId, "EnqAndItemId"));
            return (item == null ? false : true);
        }

        private async Task<IReadOnlyList<ContractReviewItem>> CreateReviewItemsOfEnquiry(int enquiryId)
        {
 
            var itemids = await _context.EnquiryItems.Where(x=>x.EnquiryId==enquiryId)
                .OrderBy(x => x.Id).Select(x => x.Id).ToListAsync();
            var reviewids = await _context.ContractReviewItems.Where(x => x.EnquiryId == enquiryId)
                .OrderBy(x => x.EnquiryItemId).Select(x => x.EnquiryItemId).ToListAsync();
            IEnumerable<int> itemlist = itemids.ToArray();
            IEnumerable<int> reviewlist = reviewids.ToArray();

            var missingids = reviewids == null ? itemlist: itemlist.Except(reviewlist);
        
            var cReviewItems = new List<ContractReviewItem>();
            foreach(var item in missingids)
            {
                cReviewItems.Add(new ContractReviewItem(item, enquiryId));
            }
            
            if (cReviewItems.Count > 0) await _unitOfWork.Repository<ContractReviewItem>().AddListAsync(cReviewItems);
            return await _unitOfWork.Repository<ContractReviewItem>().GetEntityListWithSpec(
                new ContractReviewItemSpec("dummy", enquiryId));
        }

        
    }
}