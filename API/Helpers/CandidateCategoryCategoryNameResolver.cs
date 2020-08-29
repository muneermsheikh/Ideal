using API.Dtos;
using AutoMapper;
using Core.Entities.HR;
using Core.Interfaces;

namespace API.Helpers
{
    public class CandidateCategoryCategoryNameResolver : IValueResolver<CandidateCategory, CandidateCategoryDto, string>
    {
        private readonly ICategoryService _catService;
        public CandidateCategoryCategoryNameResolver(ICategoryService catService)
        {
            _catService = catService;
        }

        public string Resolve(CandidateCategory source, CandidateCategoryDto destination, string destMember, ResolutionContext context)
        {
            var cat = _catService.CategoryByIdAsync(source.CatId).Result;
            return cat.Name; throw new System.NotImplementedException();
        }
    }
}