using API.Dtos;
using AutoMapper;
using Core.Entities.HR;
using Core.Interfaces;

namespace API.Helpers
{
    public class AssessmentCandidateNameResolver : IValueResolver<Assessment, AssessmentDto, string>
    {
        private readonly ICandidateService _candService;
        public AssessmentCandidateNameResolver(ICandidateService candService)
        {
            _candService = candService;
        }

        public string Resolve(Assessment source, AssessmentDto destination, string destMember, ResolutionContext context)
        {
            if (source.CandidateId == 0) return string.Empty;
            return _candService.GetCandidateName(source.CandidateId);
        }
    }
}