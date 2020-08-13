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
                    x.CityName.ToLower().Contains(custParams.City)) &&
                (string.IsNullOrEmpty(custParams.Email) || 
                    x.CustomerName.ToLower().Contains(custParams.Email)) &&
                (!custParams.CustomerType.HasValue || x.CustomerType == custParams.CustomerType) &&
                (!custParams.CustomerStatus.HasValue || x.CustomerStatus == custParams.CustomerStatus)) &&
                (!custParams.CustomerId.HasValue || x.Id == custParams.CustomerId))
        {
            if (custParams.IncludeOfficial) AddInclude(x => x.CustomerOfficials);
            if (custParams.IncludeAddress) AddInclude(x => x.CustomerAddress);

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
                            AddOrderBy(x => x.CityName);
                            break;
                        case "citydesc":
                            AddOrderByDescending(x => x.CityName);
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