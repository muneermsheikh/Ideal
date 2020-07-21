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

        public async Task<int> DeleteContractReviewItem(ContractReviewItem contractReviewItem)
        {
            return await _unitOfWork.Repository<ContractReviewItem>().DeleteAsync(contractReviewItem);
        }

        public async Task<int> DeleteJobDescAsync(JobDesc jobDescription)
        {
            return await _unitOfWork.Repository<JobDesc>().DeleteAsync(jobDescription);
        }

        public async Task<int> DeleteRemunerationAsync(Remuneration remuneration)
        {
            return await _unitOfWork.Repository<Remuneration>().DeleteAsync(remuneration);
        }

        public async Task<ContractReviewItem> GetContractReviewItemAsync(int EnquiryItemId)
        {
            return await _unitOfWork.Repository<ContractReviewItem>().GetByIdAsync(EnquiryItemId);
        }

        public async Task<IReadOnlyList<ContractReviewItem>> GetContractReviewItemsOfEnquiryAsync(int enquiryId)
        {
            var spec = new ContractReviewItemSpec("", enquiryId);
            return await _unitOfWork.Repository<ContractReviewItem>().GetEntityListWithSpec(spec);
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

        public async Task<JobDesc> GetJobDescOfAnItemAsync(int enquiryItemId)
        {
            var spec = new JobDescSpec(enquiryItemId);
            return await _unitOfWork.Repository<JobDesc>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<JobDesc>> GetJobDescOfEnquiryIdAsync(int enquiryId)
        {
            var spec = new JobDescSpec("",enquiryId);
            return await _unitOfWork.Repository<JobDesc>().GetEntityListWithSpec(spec);
        }

        public async Task<Remuneration> GetRemunerationAsync(int enquiryItemId)
        {
            var spec = new RemunerationSpecs(enquiryItemId);
            return await _unitOfWork.Repository<Remuneration>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Remuneration>> GetRemunerationOfEnquiry(int enquiryId)
        {
            var spec = new RemunerationSpecs("",enquiryId);
            return await _unitOfWork.Repository<Remuneration>().GetEntityListWithSpec(spec);
        }

        public async Task<ContractReviewItem> UpdateContractReviewItemAsync(ContractReviewItem contractReviewItem)
        {
            return await _unitOfWork.Repository<ContractReviewItem>().UpdateAsync(contractReviewItem);
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

        public async Task<JobDesc> UpdateJobDescAsync(JobDesc jobDesc)
        {
            return await _unitOfWork.Repository<JobDesc>().UpdateAsync(jobDesc);
        }

        public async Task<Remuneration> UpdateRemunerationAsync(Remuneration remuneration)
        {
            return await _unitOfWork.Repository<Remuneration>().UpdateAsync(remuneration);
        }
    }
}