using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.HR;
using Core.Interfaces;

namespace API.Helpers
{
    public class AssessmentCustomerResolver : IValueResolver<Assessment, AssessmentDto, string>
    {
        private readonly ICustomerService _custService;
        public AssessmentCustomerResolver(ICustomerService custService)
        {
            _custService = custService;
        }

        public string Resolve(Assessment source, AssessmentDto destination, string destMember, ResolutionContext context)
        {
            if (source.EnquiryItemId == 0) return null;
            var item = new clsString();
            item = _custService.GetCustomerFromEnquiryItemId(source.EnquiryItemId).Result;
            if (item == null) return "invalid data";
            return item.Name;
        }
    }
}