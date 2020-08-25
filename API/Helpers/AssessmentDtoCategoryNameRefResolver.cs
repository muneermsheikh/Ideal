using API.Dtos;
using AutoMapper;
using Core.Entities.HR;
using Core.Interfaces;

namespace API.Helpers
{
    public class AssessmentDtoCategoryNameRefResolver : IValueResolver<AssessmentToAddDto, Assessment, string>
    {
        private readonly ICategoryService _catService;
        private readonly IEnquiryService _enqService;
        public AssessmentDtoCategoryNameRefResolver(ICategoryService catService, IEnquiryService enqService)
        {
            _enqService = enqService;
            _catService = catService;
        }

        public string Resolve(AssessmentToAddDto source, Assessment destination, string destMember, ResolutionContext context)
        {
            if (source.EnquiryitemId == 0) return null;
            var item = _enqService.GetEnquiryItemByIdAsync(source.EnquiryitemId).Result;
            if (item == null) return null;
            var cat = _catService.CategoryByIdAsync(source.EnquiryitemId).Result;
            if (cat==null) return null;

            var enq = _enqService.GetEnquiryByIdAsync(item.EnquiryId).Result;
            if (enq==null) return null;
            
            return enq.EnquiryNo + "-" + item.SrNo + "-" + cat.Name;

        }
    }
}