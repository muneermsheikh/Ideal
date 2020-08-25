using API.Dtos;
using AutoMapper;
using Core.Entities.EnquiryAggregate;
using Core.Entities.HR;
using Core.Interfaces;

namespace API.Helpers
{
    public class AssessmentDtoEnquiryItemResolver : IValueResolver<AssessmentToAddDto, Assessment, EnquiryItem>
    {
        private readonly IEnquiryService _enqService;
        public AssessmentDtoEnquiryItemResolver(IEnquiryService enqService)
        {
            _enqService = enqService;
        }

        public EnquiryItem Resolve(AssessmentToAddDto source, Assessment destination, EnquiryItem destMember, ResolutionContext context)
        {
            if (source.EnquiryitemId==0) return null;
            var assess = _enqService.GetEnquiryItemByIdAsync(source.EnquiryitemId).Result;
            if (assess ==null) return null;
            return assess;
        }
    }
}