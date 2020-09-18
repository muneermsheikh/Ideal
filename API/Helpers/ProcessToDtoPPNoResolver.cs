using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.Processing;
using Infrastructure.Data;

namespace API.Helpers
{
    public class ProcessToDtoPPNoResolver : IValueResolver<Process, ProcessDto, string>
    {
        private readonly ATSContext _context;
        public ProcessToDtoPPNoResolver(ATSContext context)
        {
            _context = context;
        }

        public string Resolve(Process source, ProcessDto destination, string destMember, ResolutionContext context)
        {
            var candidateid = _context.CVRefs.Where(x => x.Id == source.CVRefId).Select(x => x.CandidateId).FirstOrDefault();
            return _context.Candidates.Where(x => x.Id == candidateid).Select(x => x.PPNo).FirstOrDefault();
        }
    }
}