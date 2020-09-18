using System.Linq;
using AutoMapper;
using Core.Entities.Admin;
using Infrastructure.Data;

namespace API.Helpers
{
    public class CVForwardOfficialNameResolver : IValueResolver<CVForward, CVForwardDto, string>
    {
        private readonly ATSContext _context;
        public CVForwardOfficialNameResolver(ATSContext context)
        {
            _context = context;
        }

        public string Resolve(CVForward source, CVForwardDto destination, string destMember, ResolutionContext context)
        {
            var nm = _context.CustomerOfficials.Where(x => x.Id == source.CustomerOfficialId).Select(x => x.Name).SingleOrDefault();
            if ( nm == null || nm=="") return "invalid official name";
            return nm;
        }
    }
}