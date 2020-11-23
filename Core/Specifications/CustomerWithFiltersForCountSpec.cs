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
                    x.City.ToLower().Contains(custParams.City)) &&
                (string.IsNullOrEmpty(custParams.Email) || 
                    x.CustomerName.ToLower().Contains(custParams.Email)) &&
                (string.IsNullOrEmpty(custParams.CustomerType) || 
                    x.CustomerType.ToLower() == custParams.CustomerType) &&
                (string.IsNullOrEmpty(custParams.CustomerStatus) || 
                    x.CustomerStatus.ToLower() == custParams.CustomerStatus) &&
                (!custParams.CustomerId.HasValue || x.Id == custParams.CustomerId)))
        {
        }
    }
}