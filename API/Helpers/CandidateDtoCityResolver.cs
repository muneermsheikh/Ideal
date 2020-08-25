using API.Dtos;
using AutoMapper;
using Core.Entities.HR;

namespace API.Helpers
{
    public class CandidateDtoCityResolver : IValueResolver<Candidate, CandidateDto, string>
    {
        public string Resolve(Candidate source, CandidateDto destination, string destMember, ResolutionContext context)
        {
            if (source.CandidateAddress == null) return null;
            return source.CandidateAddress.City;
        }
    }
}