using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Infrastructure.Data;
using System.Linq;

namespace API.Helpers
{
    public class CVRefCustomerCityResolver : IValueResolver<CVRef, CVRefHdrDto, string>
    {
        private readonly ATSContext _context;
        public CVRefCustomerCityResolver(ATSContext context)
        {
            _context = context;
        }

        public string Resolve(CVRef source, CVRefHdrDto destination, string destMember, ResolutionContext context)
        {
            var qry = (from i in _context.EnquiryItems join e in _context.Enquiries 
                on i.EnquiryId equals e.Id select e.Customer.City).SingleOrDefault();
            if (qry==null) return "customer city not found";
            return qry;
        }
    }
}