using System.Linq;
using AutoMapper;
using Core.Entities.Admin;
using Infrastructure.Data;

namespace API.Helpers
{
    public class CVForwardDtoCustomerNameResolver : IValueResolver<CVForward, CVForwardDto, string>
    {
        private readonly ATSContext _context;
        public CVForwardDtoCustomerNameResolver(ATSContext context)
        {
            _context = context;
        }

        public string Resolve(CVForward source, CVForwardDto destination, string destMember, ResolutionContext context)
        {
            var cust = _context.Customers.Where(x => x.Id == source.CustomerId)
                .Select(x => x.CustomerName).Take(1).SingleOrDefault();
            if (cust == null) return "Not defined";
            return cust;
        }
    }
}