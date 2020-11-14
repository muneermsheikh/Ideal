using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Infrastructure.Data;

namespace API.Helpers
{
    public class CVRefPPNoResolver : IValueResolver<CVRef, CVRefItemDto, string>
    {
        private readonly ATSContext _context;
        public CVRefPPNoResolver(ATSContext context)
        {
            _context = context;
        }

        public string Resolve(CVRef source, CVRefItemDto destination, string destMember, ResolutionContext context)
        {
            return _context.Candidates.Where(x=>x.Id==source.CandidateId).Select(x=>x.PassportNo).SingleOrDefault();
        }
    }
}