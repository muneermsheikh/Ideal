using System.Linq;
using AutoMapper;
using Core.Entities.Admin;
using Infrastructure.Data;

namespace API.Helpers
{
    public class CVForwardItemDtoAppNoResolver : IValueResolver<CVForwardItem, CVForwardItemDto, int>
    {
        private readonly ATSContext _context;
        public CVForwardItemDtoAppNoResolver(ATSContext context)
        {
            _context = context;
        }

        public int Resolve(CVForwardItem source, CVForwardItemDto destination, int destMember, ResolutionContext context)
        {
            return _context.Candidates.Where(x => x.Id == source.CandidateId).Select(x => x.ApplicationNo).FirstOrDefault();
        }
    }
}