using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> CreateCustomerAsync (Customer customerDto);
        
        Task<IReadOnlyList<Customer>> CustomerListAsync (CustomerSpecParams custParams);

        Task<Customer> CustomerByIdAsync(int customerId);
        Task<int> DeleteCustomerByIdAsync(int customerId);
        Task<int> UpdateCustomerByIdAsync(Customer customer);
        Task<int> GetCustomerIdFromEmail(CustomerSpecParams custParams);
//officials
        Task<IReadOnlyList<CustomerOfficial>> GetCustomerOfficialList(int CustomerId);
        Task<IReadOnlyList<CustomerOfficial>> InsertOfficials(List<CustomerOfficial> officials);
        Task<int> UpdateOfficials(List<CustomerOfficial> officials);
    }
}