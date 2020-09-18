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
        Task<IReadOnlyList<Enquiry>> GetDLIndexLast500Async();
        Task<Enquiry> UpdateDLAsync(Enquiry enquiry);
    
    //get customer details        
        Task<Customer> GetDLCustomer(int enquiryId);

    //dl iTEM
        Task<IReadOnlyList<EnquiryItem>> GetDLItemsAsync(int enquiryId, enumItemReviewStatus itemStatus);
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
        Task<IReadOnlyList<ContractReviewItem>> GenerateReviewItemsOfAnEnquiryAsync(int EnquiryId);
        Task<ContractReviewItem> GetOrAddReviewItemAsync(int EnquiryItemId);
        Task<IReadOnlyList<ContractReviewItem>> UpdateReviewItemsAsync(
            IReadOnlyList<ContractReviewItem> contractReviewItems);
        
        Task<int> UpdateReviewItemListAsync(List<ContractReviewItem> reviewItems);
        Task<int> DeleteContractReviewItem(ContractReviewItem contractReviewItem);
        
        Task<IReadOnlyList<ContractReviewItem>> GetOrAddReviewItemsOfEnquiryAsync(int enquiryId);


    //Remuneration
        Task<Remuneration> GetRemunerationAsync (int enquiryItemId);
        Task<Remuneration> UpdateRemunerationAsync (Remuneration remuneration);
        Task<int> DeleteRemunerationAsync (Remuneration remuneration);
        Task<IReadOnlyList<Remuneration>> GetRemunerationOfEnquiry (int enquiryId);
    }
}