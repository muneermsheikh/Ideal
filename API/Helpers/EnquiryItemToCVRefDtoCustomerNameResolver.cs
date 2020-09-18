using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Interfaces;
using Infrastructure.Data;
using System.Linq;

namespace API.Helpers
{
    public class EnquiryItemToCVRefDtoCustomerNameResolver : IValueResolver<EnquiryItem, CVRefDto, string>
    {

        private readonly ICustomerService _custService;

        public EnquiryItemToCVRefDtoCustomerNameResolver(ICustomerService custService)
        {
            _custService = custService;
        }

        public string Resolve(EnquiryItem source, CVRefDto destination, string destMember, ResolutionContext context)
        {
            var cust = _custService.GetCustomerFromEnquiryItemId(source.Id).Result;

            if (cust == null) return "customer name not found";
            return  cust.Name;
        }
    }
}