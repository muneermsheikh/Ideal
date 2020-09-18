using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Interfaces;
using Infrastructure.Data;
using System.Linq;

namespace API.Helpers
{
    public class CVRefToHistoryCustomerNameResolver : IValueResolver<CVRef, CandidateHistoryDto, string>
    {
        private readonly ICustomerService _custService;

        public CVRefToHistoryCustomerNameResolver(ICustomerService custService)
        {
            _custService = custService;
        }

        public string Resolve(CVRef source, CandidateHistoryDto destination, string destMember, ResolutionContext context)
        {
            var cust = _custService.GetCustomerFromEnquiryItemId(source.EnquiryItemId).Result;

            if (cust == null) return "customer name not found";
            return  cust.Name;
        }
    }
}