using API.Dtos;
using AutoMapper;
using Core.Entities.HR;
using Core.Interfaces;

namespace API.Helpers
{
    public class CVEvalCategoryResolver : IValueResolver<CVEvaluation, CVEvaluationDto, string>
    {
        private readonly ICategoryService _catService;
        public CVEvalCategoryResolver(ICategoryService catService)
        {
            _catService = catService;
        }

        public string Resolve(CVEvaluation source, CVEvaluationDto destination, string destMember, ResolutionContext context)
        {
            if (source.EnquiryItemId==0) return "invalid enquiry item data";
            var cat = _catService.GetCategoryNameWithRefFromEnquiryItemId(source.EnquiryItemId);
            if (string.IsNullOrEmpty(cat)) return "Invalid enquiry item data";
            return cat;
        }
    }
}