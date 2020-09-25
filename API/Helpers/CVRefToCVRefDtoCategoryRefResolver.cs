using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.Processing;
using Core.Interfaces;
using Infrastructure.Data;

namespace API.Helpers
{
    public class CVRefToCVRefDtoCategoryRefResolver : IValueResolver<CVRef, CVRefDto, string>
    {
        private readonly ICategoryService _catservice;

        public CVRefToCVRefDtoCategoryRefResolver(ICategoryService catservice)
        {
            _catservice = catservice;
        }

        public string Resolve(CVRef source, CVRefDto destination, string destMember, ResolutionContext context)
        {
            var cat = _catservice.GetCategoryNameWithRefFromEnquiryItemId(source.EnquiryItemId);
            if (string.IsNullOrEmpty(cat)) return "Invalid enquiry item data";
            return cat;
        }
    }
}