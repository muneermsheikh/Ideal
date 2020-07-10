using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.EnquiryAggregate;

namespace Core.Interfaces
{
    public interface IEnquiryService
    {
        Task<Enquiry> CreateEnquiryAsync (string buyerEmail, string basketId, Address shippingAddress);
        
        Task<IReadOnlyList<Enquiry>> GetUserEnquiriesAsync (string buyerEmail);

        Task<Enquiry> GetEnquiryById(int Id, string buyerEmail);
    }
}