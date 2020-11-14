using System.Linq;
using API.Dto;
using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Interfaces;
using Infrastructure.Data;

namespace API.Helpers
{
    public class CVRefToReferredCandidateNameResolver : IValueResolver<CVRef, HistoryDto, string>
    {
        private readonly ATSContext _context;
        public CVRefToReferredCandidateNameResolver(ATSContext context)
        {
            _context = context;
        }

        public string Resolve(CVRef source, HistoryDto destination, string destMember, ResolutionContext context)
        {
            var txt = _context.Candidates.Where(x => x.Id == source.CandidateId)
                .Select(x => new {x.FullName, x.ApplicationNo, x.PassportNo}).FirstOrDefault();
            return txt.ApplicationNo + "-" + txt.FullName + " - PP No.:" + txt.PassportNo;
        }
    }
}