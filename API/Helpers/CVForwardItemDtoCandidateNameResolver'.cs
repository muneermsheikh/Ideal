using System.Linq;
using AutoMapper;
using Core.Entities.Admin;
using Infrastructure.Data;

namespace API.Helpers
{
    public class CVForwardItemDtoCandidateNameResolver : IValueResolver<CVForwardItem, CVForwardItemDto, string>
    {
        private readonly ATSContext _context;
        public CVForwardItemDtoCandidateNameResolver(ATSContext context)
        {
            _context = context;
        }

    
        string IValueResolver<CVForwardItem, CVForwardItemDto, string>.Resolve(CVForwardItem source, CVForwardItemDto destination, string destMember, ResolutionContext context)
        {
            return _context.Candidates.Where(x => x.Id == source.CandidateId).Select(x => x.FullName).SingleOrDefault();
        }
        
    }
}