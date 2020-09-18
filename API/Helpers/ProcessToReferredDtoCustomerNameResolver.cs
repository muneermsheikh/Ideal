using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.Processing;
using Core.Interfaces;
using Infrastructure.Data;

namespace API.Helpers
{
    public class ProcessToReferredDtoCustomerNameResolver : IValueResolver<Process, ProcessReferredDto, string>
    {
        private readonly ATSContext _context;
        private readonly ICustomerService _custservice;
        public ProcessToReferredDtoCustomerNameResolver(ICustomerService custservice, ATSContext context)
        {
            _custservice = custservice;
            _context = context;
        }

        public string Resolve(Process source, ProcessReferredDto destination, string destMember, ResolutionContext context)
        {
            var enquiryitemid = _context.CVRefs.Where(x => x.Id == source.CVRefId).Select(x => x.EnquiryItemId).FirstOrDefault();
            
            var enqid = _context.EnquiryItems.Where(x=>x.Id==enquiryitemid).Select(x=>x.EnquiryId).SingleOrDefault();
            var custname = _context.Enquiries.Where(x=>x.Id==enqid)
                .Select(x=>x.Customer.CustomerName + " " + x.Customer.CityName).SingleOrDefault();
            return custname;
        }
    }
}