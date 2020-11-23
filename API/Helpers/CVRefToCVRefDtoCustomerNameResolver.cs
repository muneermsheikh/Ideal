using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.Processing;
using Core.Interfaces;
using Infrastructure.Data;

namespace API.Helpers
{
    public class CVRefToCVRefDtoCustomerNameResolver : IValueResolver<CVRef, CVRefDto, string>
    {
        private readonly ATSContext _context;
        private readonly ICustomerService _custservice;
        public CVRefToCVRefDtoCustomerNameResolver(ICustomerService custservice, ATSContext context)
        {
            _custservice = custservice;
            _context = context;
        }

        public string Resolve(CVRef source, CVRefDto destination, string destMember, ResolutionContext context)
        {
            var enqid = _context.EnquiryItems.Where(x=>x.Id==source.EnquiryItemId).Select(x=>x.EnquiryId).SingleOrDefault();
            var cust = _context.Enquiries.Where(x=>x.Id==enqid)
                .Select(x=> new {x.Customer.CustomerName, x.Customer.City}).SingleOrDefault();
            return cust.CustomerName + ", " + cust.City;
        }
    }
}