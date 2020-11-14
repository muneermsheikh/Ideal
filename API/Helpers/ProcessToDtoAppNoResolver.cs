using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.Processing;
using Infrastructure.Data;

namespace API.Helpers
{
    public class ProcessToDtoAppNoResolver : IValueResolver<Process, ProcessAddedDto, string>
    {
        private readonly ATSContext _context;
        public ProcessToDtoAppNoResolver(ATSContext context)
        {
            _context = context;
        }

        public string Resolve(Process source, ProcessAddedDto destination, string destMember, ResolutionContext context)
        {
            var v = (from rf in _context.CVRefs
                join cnd in _context.Candidates on rf.CandidateId equals cnd.Id
                where rf.Id == source.CVRefId
                select new {nm = cnd.FullName, appno = cnd.ApplicationNo, ppno = cnd.PassportNo}
                ).SingleOrDefault();
                
            if (v==null) return "invalid request";
            return v.appno + "-" + v.nm + ", PP No." + v.ppno;
        }


    }
}