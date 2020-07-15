using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class DLService : IDLService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DLService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> DeleteContractReviewItem(ContractReviewItem contractReviewItem)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> DeleteJobDescAsync(ContractReviewItem contractReviewItem)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> DeleteRemunerationAsync(Remuneration remuneration)
        {
            throw new System.NotImplementedException();
        }

        public Task<ContractReviewItem> GetContractReviewItemAsync(int EnquiryItemId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<ContractReviewItem>> GetContractReviewItemsOfEnquiryAsync(int enquiryId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Enquiry> GetDLByIdAsync(int enquiryId)
        {
            var enq = await _unitOfWork.Repository<Enquiry>().GetByIdAsync(enquiryId);
            return enq;
        }

        public async Task<IReadOnlyList<Enquiry>> GetDLIndexLast500Async()
        {
            return await _unitOfWork.Repository<Enquiry>().ListAllAsync();
        }

        public async Task<EnquiryItem> GetDLItemAsync(int enquiryItemId)
        {
            return await _unitOfWork.Repository<EnquiryItem>().GetByIdAsync(enquiryItemId);
        }

        public async Task<IReadOnlyList<EnquiryItem>> GetDLItemsAsync(int enquiryId, 
            enumItemReviewStatus itemStatus)
        {
            var spec = new EnquiryItemsSpecs(enquiryId,  itemStatus);
            return await _unitOfWork.Repository<EnquiryItem>().ListWithSpecAsync(spec);
        }

        public Task<JobDesc> GetJobDescOfAnItemAsync(int EnquiryItemId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<JobDesc>> GetJobDescOfEnquiryIdAsync(int enquiryId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Remuneration> GetRemunerationAsync(int enquiryItemId)
        {
            var spec = new RemunerationSpecs(enquiryItemId);
            return await _unitOfWork.Repository<Remuneration>().GetEntityWithSpec(spec);
        }

        public Task<IReadOnlyList<Remuneration>> GetRemunerationOfEnquiry(int enquiryId)
        {
            throw new System.NotImplementedException();
        }

        public Task<ContractReviewItem> UpdateContractReviewItemAsync(ContractReviewItem contractReviewItem)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> UpdateDLAsync(Enquiry enquiry)
        {
            await _unitOfWork.Repository<Enquiry>().UpdateAsync(enquiry);
            return await _unitOfWork.Complete();
        }

        public async Task<int> UpdateDLItemAsync(EnquiryItem enquiryItem)
        {
            await _unitOfWork.Repository<EnquiryItem>().UpdateAsync(enquiryItem);
            return await _unitOfWork.Complete();
        }

        public Task<JobDesc> UpdateJobDescAsync(ContractReviewItem contractReviewItem)
        {
            throw new System.NotImplementedException();
        }

        public Task<Remuneration> UpdateRemunerationAsync(Remuneration remuneration)
        {
            throw new System.NotImplementedException();
        }
    }
}