using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.Processing;
using Core.Interfaces;
using Infrastructure.Data;

namespace API.Helpers
{
    public class CVRefToCVRefDtoCandidateNameResolver : IValueResolver<CVRef, CVRefDto, string>
    {
        private readonly ATSContext _context;
        private readonly ICategoryService _catService;
        public CVRefToCVRefDtoCandidateNameResolver(ATSContext context)
        {
            _context = context;
        }

        public string Resolve(CVRef source, CVRefDto destination, string destMember, ResolutionContext context)
        {
            var cnd = _context.Candidates.Where(x => x.Id == source.CandidateId)
                .Select(x => new{x.ApplicationNo, x.FullName, x.PassportNo}).FirstOrDefault();
            if (cnd==null) return "undefined";
            return cnd.ApplicationNo + "-" + cnd.FullName + ", PP No." + cnd.PassportNo;
        }
    }
}