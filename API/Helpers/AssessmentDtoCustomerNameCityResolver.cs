using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.HR;
using Core.Interfaces;

namespace API.Helpers
{
    public class AssessmentDtoCustomerNameCityResolver : IValueResolver<AssessmentToAddDto, Assessment, string>
    {
        
        private readonly ICustomerService _custService;
        public AssessmentDtoCustomerNameCityResolver(ICustomerService custService)
        {
            _custService = custService;
        }

        public string Resolve(AssessmentToAddDto source, Assessment destination, string destMember, ResolutionContext context)
        {
            if (source.EnquiryitemId == 0) return null;
            var item = new clsString();
            item = _custService.GetCustomerFromEnquiryItemId(source.EnquiryitemId).Result;
                //returns customer name and city name
            if (item == null) return "invalid data";
            return item.Name;
        }
    }
}