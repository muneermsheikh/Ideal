using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.Processing;
using Infrastructure.Data;

namespace API.Helpers
{
    public class CVRefToProcessDtoPPNoResolver : IValueResolver<CVRef, ProcessDto, string>
    {
        private readonly ATSContext _context;
        public CVRefToProcessDtoPPNoResolver(ATSContext context)
        {
            _context = context;
        }

        public string Resolve(CVRef source, ProcessDto destination, string destMember, ResolutionContext context)
        {
            return _context.Candidates.Where(x => x.Id == source.CandidateId).Select(x => x.PPNo).FirstOrDefault();
        }
    }
}