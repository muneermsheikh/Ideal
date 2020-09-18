using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.Processing;
using Infrastructure.Data;

namespace API.Helpers
{
    public class ProcessToDtoAppNoResolver : IValueResolver<Process, ProcessDto, int>
    {
        private readonly ATSContext _context;
        public ProcessToDtoAppNoResolver(ATSContext context)
        {
            _context = context;
        }

        public int Resolve(Process source, ProcessDto destination, int destMember, ResolutionContext context)
        {
            var candidateid = _context.CVRefs.Where(x=>x.Id==source.CVRefId).Select(x=>x.CandidateId).SingleOrDefault();
            return _context.Candidates.Where(x=>x.Id==candidateid).Select(x=>x.ApplicationNo).SingleOrDefault();
        }
    }
}