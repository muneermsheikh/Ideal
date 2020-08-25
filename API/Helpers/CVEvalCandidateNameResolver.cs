using API.Dtos;
using AutoMapper;
using Core.Entities.HR;
using Core.Interfaces;

namespace API.Helpers
{
    public class CVEvalCandidateNameResolver : IValueResolver<CVEvaluation, CVEvaluationDto, string>
    {
        private readonly ICandidateService _candService;
        public CVEvalCandidateNameResolver(ICandidateService candService)
        {
            _candService = candService;
        }

        public string Resolve(CVEvaluation source, CVEvaluationDto destination, string destMember, ResolutionContext context)
        {
            if (source.CandidateId == 0) return string.Empty;
            return _candService.GetCandidateName(source.CandidateId);
        }
    }
}