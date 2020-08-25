using API.Dtos;
using AutoMapper;
using Core.Entities.EnquiryAggregate;
using Core.Interfaces;

namespace API.Helpers
{
    public class EnquiryItemSupNameResolver : IValueResolver<EnquiryItem, EnquiryItemToReturnDto, string>
    {
        private readonly IEmployeeService _empService;
        public EnquiryItemSupNameResolver(IEmployeeService empService)
        {
            _empService = empService;
        }

        public string Resolve(EnquiryItem source, EnquiryItemToReturnDto destination, string destMember, ResolutionContext context)
        {
            if(source.AssessingSupId==null) return "undefined";
            return _empService.GetEmployeeName((int)source.AssessingSupId);
        }
    }
}