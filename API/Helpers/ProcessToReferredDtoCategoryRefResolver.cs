using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.Processing;
using Core.Interfaces;
using Infrastructure.Data;

namespace API.Helpers
{
    public class ProcessToReferredDtoCategoryRefResolver : IValueResolver<Process, ProcessReferredDto, string>
    {
        private readonly ICategoryService _catservice;
        private readonly ATSContext _context;
        public ProcessToReferredDtoCategoryRefResolver(ICategoryService catservice, ATSContext context)
        {
            _context = context;
            _catservice = catservice;
        }

        public string Resolve(Process source, ProcessReferredDto destination, string destMember, ResolutionContext context)
        {
            var enquiryitemid = _context.CVRefs.Where(x => x.Id == source.CVRefId).Select(x => x.EnquiryItemId).FirstOrDefault();
            var cat = _catservice.GetCategoryNameWithRefFromEnquiryItemId(enquiryitemid);
            if (string.IsNullOrEmpty(cat)) return "Invalid enquiry item data";
            return cat;
        }
    }
}