using System;
using System.Linq.Expressions;
using Core.Entities.Admin;

namespace Core.Specifications
{
    public class CustomerWithFiltersForCountSpec : BaseSpecification<Customer>
    {
        public CustomerWithFiltersForCountSpec(CustomerSpecParams custParams)
            :  base( x => (
                (string.IsNullOrEmpty(custParams.Search) || 
                    x.CustomerName.ToLower().Contains(custParams.Search)) &&
                (string.IsNullOrEmpty(custParams.City) || 
                    x.CityName.ToLower().Contains(custParams.City)) &&
                (string.IsNullOrEmpty(custParams.Email) || 
                    x.CustomerName.ToLower().Contains(custParams.Email)) &&
                (!custParams.CustomerType.HasValue || x.CustomerType == custParams.CustomerType) &&
                (!custParams.CustomerStatus.HasValue || x.CustomerStatus == custParams.CustomerStatus)) &&
                (!custParams.CustomerId.HasValue || x.Id == custParams.CustomerId))
        {
        }
    }
}