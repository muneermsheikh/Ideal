using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.Processing;
using Infrastructure.Data;

namespace API.Helpers
{
    public class CVRefToProcessDtoAppNoResolver : IValueResolver<CVRef, ProcessDto, int>
    {
        private readonly ATSContext _context;
        public CVRefToProcessDtoAppNoResolver(ATSContext context)
        {
            _context = context;
        }

        public int Resolve(CVRef source, ProcessDto destination, int destMember, ResolutionContext context)
        {
            return _context.Candidates.Where(x=>x.Id==source.CandidateId).Select(x=>x.ApplicationNo).SingleOrDefault();
        }
    }
}