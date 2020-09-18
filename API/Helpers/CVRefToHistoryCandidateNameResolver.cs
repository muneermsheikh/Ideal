using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Interfaces;

namespace API.Helpers
{
    public class CVRefToHistoryCandidateNameResolver : IValueResolver<CVRef, CandidateHistoryDto, string>
    {
        private readonly ICandidateService _candService;
        public CVRefToHistoryCandidateNameResolver(ICandidateService candService)
        {
            _candService = candService;
        }

        public string Resolve(CVRef source, CandidateHistoryDto destination, string destMember, ResolutionContext context)
        {
            if (source.CandidateId == 0) return string.Empty;
            return _candService.GetCandidateName(source.CandidateId);
        }
    }
}