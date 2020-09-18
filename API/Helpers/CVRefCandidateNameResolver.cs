using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Interfaces;

namespace API.Helpers
{
    public class CVRefCandidateNameResolver : IValueResolver<CVRef, CVRefItemDto, string>
    {
        private readonly ICandidateService _candService;
        public CVRefCandidateNameResolver(ICandidateService candService)
        {
            _candService = candService;
        }

        public string Resolve(CVRef source, CVRefItemDto destination, string destMember, ResolutionContext context)
        {
            if (source.CandidateId == 0) return string.Empty;
            return _candService.GetCandidateName(source.CandidateId);
        }
    }
}