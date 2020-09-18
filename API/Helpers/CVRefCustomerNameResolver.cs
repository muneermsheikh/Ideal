using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Interfaces;
using Infrastructure.Data;
using System.Linq;

namespace API.Helpers
{
    public class CVRefCustomerNameResolver : IValueResolver<CVRef, CVRefHdrDto, string>
    {
        private readonly ICustomerService _custService;

        public CVRefCustomerNameResolver(ICustomerService custService)
        {
            _custService = custService;
        }

        public string Resolve(CVRef source, CVRefHdrDto destination, string destMember, ResolutionContext context)
        {
            var cust = _custService.GetCustomerFromEnquiryItemId(source.EnquiryItemId).Result;

            if (cust == null) return "customer name not found";
            return  cust.Name;
        }
    }
}