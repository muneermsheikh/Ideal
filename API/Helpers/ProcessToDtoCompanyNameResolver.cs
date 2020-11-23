using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.Processing;
using Core.Interfaces;
using Infrastructure.Data;

namespace API.Helpers
{
    public class ProcessToDtoCompanyNameResolver : IValueResolver<Process, ProcessAddedDto, string>
    {

        private readonly ATSContext _context;

        public ProcessToDtoCompanyNameResolver(ATSContext context)
        {
            _context = context;
        }

        public string Resolve(Process source, ProcessAddedDto destination, string destMember, ResolutionContext context)
        {
            var enquiryitemid = _context.CVRefs.Where(x => x.Id == source.CVRefId)
                .Select(x => x.EnquiryItemId).FirstOrDefault();

            var enqid = _context.EnquiryItems.Where(x => x.Id == enquiryitemid)
                .Select(x => x.EnquiryId).SingleOrDefault();
            var custname = _context.Enquiries.Where(x => x.Id == enqid)
                .Select(x => x.Customer.CustomerName + " " + x.Customer.City).SingleOrDefault();
            return custname;

        }
    }
}