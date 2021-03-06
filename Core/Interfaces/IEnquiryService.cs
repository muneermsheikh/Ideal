using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;

using Core.Entities.Masters;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IEnquiryService
    {
        Task<Enquiry> CreateEnquiryAsync (string basketId);
        Task<Enquiry> AddEnquiryAsync (Enquiry enq);
        Task<Enquiry> UpdateDLAsync (Enquiry enq);
        Task<int> DeleteEnquiryAsync (int Id);
        Task<IReadOnlyList<Enquiry>> GetUserEnquiriesAsync (int CustomerId);
        Task<IReadOnlyList<Enquiry>> GetEntityListWithSpec(EnquiryParams enqParam);
        Task<IReadOnlyList<Enquiry>> GetEnquiryList500WithAllStatus();
        Task<Enquiry> GetEnquiryWithSpecByIdAsync(int enquiryId);
        Task<Enquiry> GetEnquiryByIdAsync(int enquiryId);
        Task<Enquiry> GetEnquiryByEnquiryItemIdAsync(int enquiryItemId);
        Task<EnquiryItem> GetEnquiryItemByIdAsync(int enquiryItemId);
        Task<int> GetEnquiryItemsCountNotReviewed(int enquiryId);
        

        // Task<IReadOnlyList<Enquiry>> DLIndexTop500 ();

    //employee
        Task<Employee> EmployeeToReturn(int employeeId);
    
    //JD
        Task<JobDesc> GetJobDescriptionBySpecAsync(int enquiryItemId);
        Task<JobDesc> UpdateJDAsync(JobDesc jobDesc);
    
    //remunerations
        //flg also creates new record if one does not exist
        Task<Remuneration> GetRemunerationBySpecEnquiryItemIdAsync (int enquiryItemId);
        Task<Remuneration> UpdateRemunerationAsync(Remuneration remuneration);
        Task<IReadOnlyList<Remuneration>> GetRemunerationsEnquiryAsync(int enquiryId);
        Task<int> UpdateRemunerationsAsync(List<Remuneration> remunerations);
    //contract review
        
        Task<bool> UpdateEnquiryReadyToReview(Enquiry enquiry);
        
// category orderno details
        Task<CategoryRefFromEnquiryItemId> GetDetailsFromEnquiryItemId(int enquiryItemId);        
    }
}