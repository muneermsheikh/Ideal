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
        Task<EnquiryItem> GetEnquiryItemByIdAsync(int enquiryItemId);
        Task<int> GetEnquiryItemsCountNotReviewed(int enquiryId);

    //JD
        Task<JobDesc> GetJobDescriptionBySpecAsync(int enquiryItemId);
        Task<JobDesc> UpdateJDAsync(JobDesc jobDesc);
    
    //remunerations
        //flg also creates new record if one does not exist
        Task<Remuneration> GetRemunerationBySpecEnquiryItemIdAsync (int enquiryItemId);
        Task<Remuneration> UpdateRemunerationAsync(Remuneration remuneration);

     
        
        
    //contract review
        Task<ContractReviewItem> GetContractReviewItemAsync(int enquiryItemId);
        Task<ContractReviewItem> UpdateContractReviewItemAsync(ContractReviewItem contractReviewItem);
        Task<bool> UpdateEnquiryReadyToReview(Enquiry enquiry);
        
        
        
        




    }
}