using API.Dtos;
using AutoMapper;
using Core.Entities.HR;
using Core.Interfaces;

namespace API.Helpers
{
    public class AssessmentDtoCustomerNameCityResolver : IValueResolver<AssessmentToAddDto, Assessment, string>
    {
        private readonly IDLService _dlService;
        public AssessmentDtoCustomerNameCityResolver(IDLService dlService)
        {
            _dlService = dlService;
        }

        public string Resolve(AssessmentToAddDto source, Assessment destination, string destMember, ResolutionContext context)
        {
            if (source.EnquiryitemId == 0) return null;
            var item = _dlService.GetDLItemAsync(source.EnquiryitemId).Result;
            if (item==null) return null;
            var enqId = item.EnquiryId;
            var cust = _dlService.GetDLCustomer(enqId).Result;
            return cust.CustomerName + ", " + cust.CityName;
        }
    }
}