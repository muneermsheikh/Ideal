using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            var cust = await _unitOfWork.Repository<Customer>().UpdateAsync(customer);
            var result = await _unitOfWork.Complete();
            if (result == 0) return null;
            return customer;
        }

        public async Task<Customer> CustomerByIdAsync(int customerId)
        {
            // var spec = new CustomerSpecs(customerId);
            return await _unitOfWork.Repository<Customer>().GetByIdAsync(customerId);
        }

        public async Task<IReadOnlyList<Customer>> CustomerListAsync(CustomerSpecParams sParams)
        {
            var spec = new CustomerSpecs(sParams);
            return await _unitOfWork.Repository<Customer>().ListWithSpecAsync(spec);
        }

        public async Task<int> DeleteCustomerByIdAsync(int customerId)
        {
            var cust = await _unitOfWork.Repository<Customer>().GetByIdAsync(customerId);
            return  await _unitOfWork.Complete();
        }

        public async Task<int> GetCustomerIdFromEmail(CustomerSpecParams custParams)
        {
            var spec = new CustomerSpecs(custParams);
            var cust = await _unitOfWork.Repository<Customer>().GetEntityWithSpec(spec);
            if (cust == null) return 0;
            return cust.Id;
        }

        public async Task<int> UpdateCustomerByIdAsync(Customer customer)
        {
            var cust = await _unitOfWork.Repository<Customer>().UpdateAsync(customer);
            return await _unitOfWork.Complete();
        }

        public async Task<Customer> GetCustomerFromEmailAsync(string email)
        {
            return await _unitOfWork.Repository<Customer>().GetCustomerFromEmailAsync(email);
        }
    }
}