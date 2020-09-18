using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Interfaces;

namespace API.Helpers
{
    public class EnquiryItemToCVRefDtoCategoryRefResolver : IValueResolver<EnquiryItem, CVRefDto, string>
    {
        private readonly ICategoryService _catService;
        public EnquiryItemToCVRefDtoCategoryRefResolver (ICategoryService catService)
        {
            _catService = catService;
        }

        public string Resolve(EnquiryItem source, CVRefDto destination, string destMember, ResolutionContext context)
        {
            var cat = _catService.GetCategoryNameWithRefFromEnquiryItemId(source.Id);
            if (string.IsNullOrEmpty(cat)) return "Invalid enquiry item data";
            return cat;
        }
    }
}