using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<clsString> GetCustomerFromEnquiryItemId(int EnquiryItemId);
        Task<IReadOnlyList<Customer>> CustomerListAsync (CustomerSpecParams custParams);

        Task<Customer> CustomerByIdAsync(int customerId);
        Task<int> DeleteCustomerByIdAsync(int customerId);
        Task<Customer> UpdateCustomer(Customer customer);
        Task<int> GetCustomerIdFromEmail(CustomerSpecParams custParams);
        Task<string> CustomerCountryCurrency(int customerId);
        Task<IReadOnlyList<Customer>> GetCustomerListFlat(string customerType);
        
//officials
        Task<IReadOnlyList<CustomerOfficial>> GetCustomerOfficialList(int CustomerId);
        Task<IReadOnlyList<CustomerOfficial>> GetAllOfficialList();
        Task<IReadOnlyList<CustomerOfficial>> InsertOfficials(List<CustomerOfficial> officials);
        Task<int> UpdateOfficials(List<CustomerOfficial> officials);
    
    //recruitment agencies
        Task<IReadOnlyList<Customer>> GetAgencies();
    }
}