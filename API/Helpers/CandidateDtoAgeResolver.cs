using System;
using API.Dtos;
using AutoMapper;
using Core.Entities.HR;

namespace API.Helpers
{
    public class CandidateDtoAgeResolver : IValueResolver<Candidate, CandidateDto, int>
    {
        public int Resolve(Candidate source, CandidateDto destination, int destMember, ResolutionContext context)
        {
            if(source.DateOfBirth == null) return 0;
            return DateTime.Now.Year - source.DateOfBirth.Year;
        }
    }
}