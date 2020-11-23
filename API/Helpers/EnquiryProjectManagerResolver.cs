using API.Dtos;
using AutoMapper;
using Core.Entities.EnquiryAggregate;
using Core.Interfaces;

namespace API.Helpers
{
    public class EnquiryProjectManagerResolver : IValueResolver<Enquiry, EnquiryWithAllStatusDto, string>
    {
         private readonly IEmployeeService _empService;
        public EnquiryProjectManagerResolver(IEmployeeService empService)
        {
            _empService = empService;
        }

        public string Resolve(Enquiry source, EnquiryWithAllStatusDto destination, string destMember, ResolutionContext context)
        {
            if(source.ProjectManagerId==0) return "undefined";
            return _empService.GetEmployeeName((int)source.ProjectManagerId);
        }
    }
}
