using System;
using System.Linq.Expressions;
using Core.Entities.Admin;
using Core.Enumerations;

namespace Core.Specifications
{
    public class CustomerSpecs : BaseSpecification<Customer>
    {
        public CustomerSpecs(CustomerSpecParams custParams)
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
            if (custParams.IncludeOfficial) AddInclude(x => x.CustomerOfficials);
            if (custParams.IncludeIndustryTypes) AddInclude(x => x.CustomerIndustryTypes);

            ApplyPaging(custParams.PageSize * (custParams.PageIndex-1), custParams.PageSize);

                if (!string.IsNullOrEmpty(custParams.Sort))
                {
                    switch (custParams.Sort.ToLower())
                    {
                        case "customername":
                            AddOrderBy(x => x.CustomerName);
                            break;
                        case "customernamedesc":
                            AddOrderByDescending(x => x.CustomerName);
                            break;
                        case "city":
                            AddOrderBy(x => x.City);
                            break;
                        case "citydesc":
                            AddOrderByDescending(x => x.City);
                            break;
                        case "customerstatus":
                            AddOrderBy(x => x.CustomerStatus);
                            break;
                        case "customerstatusdesc":
                            AddOrderBy(x => x.CustomerStatus);
                            break;
                        case "customertype":
                            AddOrderBy(x => x.CustomerType);
                            break;
                        case "customertypedesc":
                            AddOrderBy(x => x.CustomerType);
                            break;
                        default:
                            AddOrderBy(x => x.CustomerName);
                            break;
                }
            }
        }
    }
}