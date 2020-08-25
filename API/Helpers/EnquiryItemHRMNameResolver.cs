using API.Dtos;
using AutoMapper;
using Core.Entities.EnquiryAggregate;
using Core.Interfaces;

namespace API.Helpers
{
    public class EnquiryItemHRMNameResolver : IValueResolver<EnquiryItem, EnquiryItemToReturnDto, string>
    {
        private readonly IEmployeeService _empService;
        public EnquiryItemHRMNameResolver(IEmployeeService empService, ICategoryService catService)
        {
            _empService = empService;
        }

        public string Resolve(EnquiryItem source, EnquiryItemToReturnDto destination, string destMember, ResolutionContext context)
        {
            if(source.AssessingHRMId==null) return "undefined";
            return _empService.GetEmployeeName((int)source.AssessingHRMId);
        }
    }
}