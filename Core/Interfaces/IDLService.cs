using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;

namespace Core.Interfaces
{
    public interface IDLService
    {
         
        Task<Enquiry> GetDLByIdAsync (int enquiryId);
        
        Task<IReadOnlyList<Enquiry>> GetDLIndexLast500Async();
        Task<IReadOnlyList<EnquiryItem>> GetDLItemsAsync(int enquiryId, enumItemReviewStatus itemStatus);
        Task<EnquiryItem> GetDLItemAsync(int enquiryItemId);
        Task<int> UpdateDLAsync(Enquiry enquiry);
        Task<int> UpdateDLItemAsync(EnquiryItem enquiryItem);
    // JD
        Task<JobDesc> GetJobDescOfAnItemAsync(int EnquiryItemId);
        Task<JobDesc> UpdateJobDescAsync(JobDesc jobDesc);
        Task<int> DeleteJobDescAsync(JobDesc jobDescription);
        Task<IReadOnlyList<JobDesc>> GetJobDescOfEnquiryIdAsync(int enquiryId);

    //contract review item
        Task<ContractReviewItem> GetContractReviewItemAsync(int EnquiryItemId);
        Task<ContractReviewItem> UpdateContractReviewItemAsync(ContractReviewItem contractReviewItem);
        Task<int> DeleteContractReviewItem(ContractReviewItem contractReviewItem);
        Task<IReadOnlyList<ContractReviewItem>> GetContractReviewItemsOfEnquiryAsync( int enquiryId);
        
    //Remuneration
        Task<Remuneration> GetRemunerationAsync (int enquiryItemId);
        Task<Remuneration> UpdateRemunerationAsync (Remuneration remuneration);
        Task<int> DeleteRemunerationAsync (Remuneration remuneration);
        Task<IReadOnlyList<Remuneration>> GetRemunerationOfEnquiry (int enquiryId);
    }
}