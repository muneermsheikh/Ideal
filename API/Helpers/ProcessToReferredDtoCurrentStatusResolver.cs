using System;
using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.Processing;
using Core.Enumerations;
using Infrastructure.Data;

namespace API.Helpers
{
    public class ProcessToReferredDtoCurrentStatusResolver : IValueResolver<Process, ProcessReferredDto, string>
    {
        private readonly ATSContext _context;
        public ProcessToReferredDtoCurrentStatusResolver(ATSContext context)
        {
            _context = context;
        }

        public string Resolve(Process source, ProcessReferredDto destination, string destMember, ResolutionContext context)
        {
            var statusid = _context.Processes.Where(x=>x.CVRefId==source.CVRefId)
                .OrderByDescending(x=>x.ProcessingDate).Take(1).Select(x=>x.Status).SingleOrDefault();
            return Enum.GetName(typeof(enumProcessingStatus), statusid);
        }
    }
}