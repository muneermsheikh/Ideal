using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.HR;
using Core.Interfaces;
using Infrastructure.Data;

namespace API.Helpers
{
    public class AssessmentApplicationNoResolver : IValueResolver<Assessment, AssessmentDto, int>
    {
        private readonly ATSContext _context;
        public AssessmentApplicationNoResolver(ATSContext context)
        {
            _context = context;

        }

        public int Resolve(Assessment source, AssessmentDto destination, int destMember, ResolutionContext context)
        {
            return _context.Candidates.Where(x=>x.Id==source.CandidateId).Select(x=>x.ApplicationNo).FirstOrDefault();
        }
    }
}