using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.Dtos;
using Core.Entities.EnquiryAggregate;

namespace Core.Interfaces
{
    public interface IEnquiryService
    {
        Task<Enquiry> CreateEnquiryAsync (string buyerEmail, string basketId, SiteAddress shippingAddress);
        
        Task<IReadOnlyList<Enquiry>> GetUserEnquiriesAsync (string buyerEmail);
        
        Task<Enquiry> GetEnquiryById(int Id, string buyerEmail);
        Task<Enquiry> GetEnquiryByIdAsync(int enquiryId);
        
        Task<JobDesc> GetJobDescriptionAsync(int enquiryItemId);
        Task<Remuneration> GetRemunerationAsync (int enquiryItemId);
        
        Task<int> GetEnquiryItemsCountNotReviewed(int enquiryId);

        
        Task<int> UpdateJDAsync(JobDesc jobDesc);
        Task<int> UpdateRemunerationAsync(Remuneration remuneration);
        Task<int> UpdateContractReviewItemAsync(ContractReviewItem contractReviewItem);
        Task<bool> UpdateEnquiryReadyToReview(Enquiry enquiry);
        
        Task<int> DeleteEnquiryForwarded(int enquiryItem, DateTime dateForwarded, int associateOfficialId);
        
        




    }
}