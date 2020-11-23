using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.EnquiryAggregate;
using Infrastructure.Data;

namespace API.Helpers
{
    public class EnquiryCustomerNameResolver : IValueResolver<Enquiry, EnquiryWithAllStatusDto, string>
    {
        private readonly ATSContext _context;
        public EnquiryCustomerNameResolver(ATSContext context)
        {
            _context = context;
        }

        public string Resolve(Enquiry source, EnquiryWithAllStatusDto destination, string destMember, ResolutionContext context)
        {
            var cust =  _context.Customers.Where(x=>x.Id==source.CustomerId)
                .Select(x => x.CustomerName).SingleOrDefault();
            return cust;
        }
    }
}