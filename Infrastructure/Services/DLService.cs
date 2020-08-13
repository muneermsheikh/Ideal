using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Masters;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class DLService : IDLService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<EnquiryItem> _itemRepo;
        private readonly IEmployeeService _empService;
        public DLService(IUnitOfWork unitOfWork, IGenericRepository<EnquiryItem> itemRepo, 
            IEmployeeService empService)
        {
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
            return await _unitOfWork.Repository<Enquiry>().ListAllAsync();
        }

        public async Task<Enquiry> UpdateDLAsync(Enquiry enquiry)
        {
            return await _unitOfWork.Repository<Enquiry>().UpdateAsync(enquiry);
        }

        public async Task<Customer> GetDLCustomer(int enquiryId)
        {
            var enq = await _unitOfWork.Repository<Enquiry>().GetByIdAsync(enquiryId);
            if(enq==null) return null;
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

        public async Task<EnquiryItem> AddDLItemAsync(EnquiryItem enquiryItem)
        {
            return await _itemRepo.AddAsync(enquiryItem);
        }

        public async Task<bool> DeleteDLItemAsync(EnquiryItem enquiryItem)
        {
            return await _itemRepo.DeleteAsync(enquiryItem) == 0 ? false : true;
        }

        public async Task<Category> GetEnquiryItemCategory (int enquiryItemId)
        {
            var item = await _itemRepo.GetByIdAsync(enquiryItemId);
            if (item==null) return null;
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
            var item = await _unitOfWork.Repository<ContractReviewItem>()
                .GetEntityWithSpec(new ContractReviewItemSpec(enquiryItemId));
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
            var rvws =  await _unitOfWork.Repository<ContractReviewItem>()
                .GetEntityListWithSpec(new ContractReviewItemSpec("", enquiryId));
            if (rvws.Count == 0) return await CreateReviewItemsOfEnquiry(enquiryId);
            
            return rvws;
        }


        public async Task<ContractReviewItem> UpdateReviewItemAsync(ContractReviewItem reviewItem)
        {
            return await _unitOfWork.Repository<ContractReviewItem>().UpdateAsync(reviewItem);
        }

        public async Task<int> UpdateReviewItemListAsync(List<ContractReviewItem> reviewItems)
        {
            return await _unitOfWork.Repository<ContractReviewItem>().UpdateListAsync(reviewItems);
        }
        
        public async Task<IReadOnlyList<ContractReviewItem>> UpdateReviewItemsAsync(
            IReadOnlyList<ContractReviewItem> contractReviewItems)
        {
            var items = new List<ContractReviewItem>();
            //var enqId = contractReviewItems[0].EnquiryId;

            foreach(var item in contractReviewItems)
            {
                //if(item.EnquiryId != enqId) return null;
                if(!await EnquiryItemIdMatchesWithEnquiryId(item.Id, item.EnquiryId)) return null;
            }

            var _repoReview = _unitOfWork.Repository<ContractReviewItem>();
            foreach(var item in contractReviewItems)
            {
                var rvwExists = await _repoReview.GetEntityWithSpec(new ContractReviewItemSpec(item.EnquiryItemId));
                if (rvwExists == null)
                {
                    rvwExists = await _repoReview.AddAsync(item);
                    if (rvwExists != null) items.Add(rvwExists);
                } else {
                    var rvw = await _repoReview.UpdateAsync(item);
                    if (rvw !=null) items.Add(rvw);
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
            return (item == null ? false: true);
        }

        private async Task<IReadOnlyList<ContractReviewItem>> CreateReviewItemsOfEnquiry(int enquiryId)
        {
            var cReviewItemsList = new List<ContractReviewItem>();

            var _repoItem = _unitOfWork.Repository<EnquiryItem>();
            var enqItems = await _repoItem.GetEntityListWithSpec(
                new EnquiryItemsSpecs(enquiryId, enumItemReviewStatus.Accepted));
            var _repoReview = _unitOfWork.Repository<ContractReviewItem>();
            foreach (var eItem in enqItems)
            {
                var item = await _repoReview.GetEntityWithSpec(new ContractReviewItemSpec(eItem.Id));
                if (item == null)
                {
                    var reviewItem = await _repoReview.AddAsync(new ContractReviewItem(eItem.Id, enquiryId));
                    if (reviewItem != null) cReviewItemsList.Add(reviewItem);
                }
            }

            return cReviewItemsList;

        }
    }
}