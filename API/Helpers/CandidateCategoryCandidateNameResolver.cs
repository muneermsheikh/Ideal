using API.Dtos;
using AutoMapper;
using Core.Entities.HR;
using Core.Interfaces;

namespace API.Helpers
{
    public class CandidateCategoryCandidateNameResolver : IValueResolver<CandidateCategory, CandidateCategoryDto, string>
    {
        private readonly ICandidateService _candService;
        public CandidateCategoryCandidateNameResolver(ICandidateService candService)
        {
            _candService = candService;
        }

        public string Resolve(CandidateCategory source, CandidateCategoryDto destination, string destMember, ResolutionContext context)
        {
            if (source.CandId == 0) return string.Empty;
            return _candService.GetCandidateName(source.CandId);
        }
    }
}