using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Interfaces;

namespace API.Helpers
{
    public class ReviewItemCustomerNameResolver: IValueResolver<ContractReviewItem, ReviewItemDto, string>
    {
        private readonly IDLService _dLService;
        private readonly Customer _customer;
        public ReviewItemCustomerNameResolver(IDLService dLService)
        {
            _dLService = dLService;
        }

        public string Resolve(ContractReviewItem source, ReviewItemDto destination, string destMember, ResolutionContext context)
        {
            if (source.EnquiryId != 0)
            {
                var cust = _dLService.GetDLCustomer(source.EnquiryId).Result;
                return cust.CustomerName;
            }

            return null;
        }
    }
}