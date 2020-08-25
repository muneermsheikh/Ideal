using API.Dtos;
using AutoMapper;
using Core.Entities.HR;
using Core.Entities.Masters;
using Core.Interfaces;

namespace API.Helpers
{
    public class CategoryNameResolver : IValueResolver<CandidateCategory, CategoryNameDto, string>
    {
        private readonly ICategoryService _catService;
        public CategoryNameResolver(ICategoryService catService)
        {
            _catService = catService;
        }

        public string Resolve(CandidateCategory source, CategoryNameDto destination, string destMember, ResolutionContext context)
        {
            var cat = _catService.CategoryByIdAsync(source.CatId).Result;
            return cat.Name;
        }
    }
}