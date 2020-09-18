using API.Dtos;
using AutoMapper;
using Core.Entities.HR;
using Core.Interfaces;

namespace API.Helpers
{
    public class AssessmentCategoryResolver : IValueResolver<Assessment, AssessmentDto, string>
    {
        private readonly ICategoryService _catService;
        public AssessmentCategoryResolver(ICategoryService catService)
        {
            _catService = catService;
        }

        public string Resolve(Assessment source, AssessmentDto destination, string destMember, ResolutionContext context)
        {
            if (source.EnquiryItemId==0) return "Invalid Enquiry Item";
            return _catService.GetCategoryNameWithRefFromEnquiryItemId(source.EnquiryItemId);
        }
    }
}