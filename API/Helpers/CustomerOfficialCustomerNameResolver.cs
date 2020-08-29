using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Infrastructure.Data;

namespace API.Helpers
{
    public class CustomerOfficialCustomerNameResolver : IValueResolver<CustomerOfficial, CustomerOfficialDto, string>
    {
        private readonly ATSContext _context;
        public CustomerOfficialCustomerNameResolver(ATSContext context)
        {
            _context = context;
        }

        public string Resolve(CustomerOfficial source, CustomerOfficialDto destination, string destMember, ResolutionContext context)
        {
            var cust = _context.Customers.Where(x => x.Id == source.CustomerId)
                .Select(x => x.CustomerName).Take(1).SingleOrDefault();
            if (cust==null) return "Not defined";
            return cust;
        }
    }
}