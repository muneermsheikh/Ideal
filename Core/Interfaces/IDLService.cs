using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Masters;
using Core.Enumerations;

namespace Core.Interfaces
{
    public interface IDLService
    {

        Task<Enquiry> GetDLByIdAsync (int enquiryId);
        Task<IReadOnlyList<Enquiry>> GetEnquiryList500WithAllStatus();
        Task<Enquiry> UpdateDLAsync(Enquiry enquiry);
    
    //get customer details        
        Task<Customer> GetDLCustomer(int enquiryId);

    //dl iTEM
        Task<IReadOnlyList<EnquiryItem>> GetDLItemsAsync(int enquiryId, string itemStatus);
        Task<EnquiryItem> GetDLItemAsync(int enquiryItemId);
        Task<EnquiryItem> UpdateDLItemAsync(EnquiryItem enquiryItem);
        Task<bool> DeleteDLItemAsync(EnquiryItem enquiryItem);
        Task<IReadOnlyList<EnquiryItem>> AddDLItemsAsync(List<EnquiryItem> enquiryItems);
        Task<Category> GetEnquiryItemCategory (int enquiryItemId);


    // JD
        Task<JobDesc> GetJobDescOfAnItemAsync(int EnquiryItemId);
        Task<JobDesc> UpdateJobDescAsync(JobDesc jobDesc);
        Task<int> DeleteJobDescAsync(JobDesc jobDescription);
        Task<IReadOnlyList<JobDesc>> GetJobDescOfEnquiryIdAsync(int enquiryId);

    //contract review item
        
        Task<ContractReview> GetContractReviewAsync (int enquiryId);
        Task<ContractReviewItem> GetOrAddReviewItemAsync(int EnquiryItemId);
        Task<IReadOnlyList<ContractReviewItem>> UpdateReviewItemsAsync(
            IReadOnlyList<ContractReviewItem> contractReviewItems);
        Task<ContractReview> UpdateReviewAsync(ContractReview review);
        Task<int> UpdateReviewStatusInEnqItemsEnqCReviewAsync(List<ContractReviewItem> reviewItems);
        Task<int> DeleteContractReviewItem(ContractReviewItem contractReviewItem);

        
    //Remuneration
        Task<Remuneration> GetRemunerationAsync (int enquiryItemId);
        Task<Remuneration> UpdateRemunerationAsync (Remuneration remuneration);
        Task<int> DeleteRemunerationAsync (Remuneration remuneration);
        Task<IReadOnlyList<Remuneration>> GetRemunerationOfEnquiry (int enquiryId);
    }
}