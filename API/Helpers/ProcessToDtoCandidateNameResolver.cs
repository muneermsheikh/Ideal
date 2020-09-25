using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.Processing;
using Core.Interfaces;
using Infrastructure.Data;

namespace API.Helpers
{
    public class ProcessToDtoCandidateNameResolver : IValueResolver<Process, ProcessAddedDto, string>
    {
        private readonly ATSContext _context;
        private readonly ICategoryService _catService;
        public ProcessToDtoCandidateNameResolver(ICategoryService catService, ATSContext context)
        {
            _catService = catService;
            _context = context;
        }

        public string Resolve(Process source, ProcessAddedDto destination, string destMember, ResolutionContext context)
        {
            var candidateid = _context.CVRefs.Where(x => x.Id == source.CVRefId).Select(x => x.CandidateId).FirstOrDefault();
            return _context.Candidates.Where(x => x.Id == candidateid).Select(x => x.FullName).FirstOrDefault();
        }
    }
}