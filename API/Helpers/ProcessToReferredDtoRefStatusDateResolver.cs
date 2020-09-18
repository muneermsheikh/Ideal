using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.Processing;
using Infrastructure.Data;

namespace API.Helpers
{
    public class ProcessToReferredDtoRefStatusDateResolver : IValueResolver<Process, ProcessReferredDto, string>
    {
        private readonly ATSContext _context;
        public ProcessToReferredDtoRefStatusDateResolver(ATSContext context)
        {
            _context = context;
        }

        public string Resolve(Process source, ProcessReferredDto destination, string destMember, ResolutionContext context)
        {
            return _context.CVRefs.Where(x=>x.Id == source.CVRefId).Select(x=>x.DateForwarded).ToString();
        }
    }
}