using AutoMapper;
using Core.Entities.Admin;
using Core.Interfaces;

namespace API.Helpers
{
    public class CVForwardItemDtoCategoryRefResolver : IValueResolver<CVForwardItem, CVForwardItemDto, string>
    {
        private readonly ICategoryService _catService;
        public CVForwardItemDtoCategoryRefResolver(ICategoryService catService)
        {
            _catService = catService;
        }

        public string Resolve(CVForwardItem source, CVForwardItemDto destination, string destMember, ResolutionContext context)
        {
            if (source.EnquiryItemId == 0) return "invalid enquiry item data";
            var cat = _catService.GetCategoryNameWithRefFromEnquiryItemId(source.EnquiryItemId);
            if (string.IsNullOrEmpty(cat)) return "Invalid enquiry item data";
            return cat;
        }
    }
}