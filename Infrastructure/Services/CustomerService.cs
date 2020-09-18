using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDLService _dLService;
        private readonly ATSContext _context;

        public CustomerService(IUnitOfWork unitOfWork, IDLService dLService, ATSContext context)
        {
            _dLService = dLService;
            _context = context;
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
            return await _unitOfWork.Complete();
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

        //officials
        public async Task<IReadOnlyList<CustomerOfficial>> GetCustomerOfficialList(int CustomerId)
        {
            return await _unitOfWork.Repository<CustomerOfficial>().GetEntityListWithSpec(
                new CustomerOfficialsSpecs(CustomerId));
        }

        public async Task<IReadOnlyList<CustomerOfficial>> InsertOfficials(List<CustomerOfficial> officials)
        {
            return await _unitOfWork.Repository<CustomerOfficial>().AddListAsync(officials);
        }

        public async Task<int> UpdateOfficials(List<CustomerOfficial> officials)
        {
            return await _unitOfWork.Repository<CustomerOfficial>().UpdateListAsync(officials);
        }

        public async Task<clsString> GetCustomerFromEnquiryItemId(int EnquiryItemId)
        {
            var enqid = await _context.EnquiryItems.Where(x=>x.Id==EnquiryItemId)
                .Select(x=>x.EnquiryId).SingleOrDefaultAsync();
            var custname = await _context.Enquiries.Where(x=>x.Id==enqid)
                .Select(x=>x.Customer.CustomerName + " " + x.Customer.CityName).SingleOrDefaultAsync();
            var r = new clsString();
            r.Name=custname;
            return r;
        }
    }
}