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
                (!custParams.CustomerStatus.HasValue || x.CustomerStatus == custParams.CustomerStatus))
            )
        {
            AddInclude(x => x.CustomerOfficials);

            if (custParams.PageSize != 0)
            {
                ApplyPaging(custParams.PageSize, custParams.PageSize * (custParams.PageIndex-1));

                if (!string.IsNullOrEmpty(custParams.Sort))
                {
                    switch (custParams.Sort)
                    {
                        case "CustomeryNameAsc":
                            AddOrderBy(x => x.CustomerName);
                            break;
                        case "CustoomerNameDesc":
                            AddOrderByDescending(x => x.CustomerName);
                            break;
                        case "CityAsc":
                            AddOrderBy(x => x.CityName);
                            break;
                        case "CityDesc":
                            AddOrderByDescending(x => x.CityName);
                            break;
                        case "CustomerStatusAsc":
                            AddOrderBy(x => x.CustomerStatus);
                            break;
                        case "CustomerStatusDesc":
                            AddOrderBy(x => x.CustomerStatus);
                            break;
                        case "CustomerTypeAsc":
                            AddOrderBy(x => x.CustomerType);
                            break;
                        case "CustomerTypeDesc":
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
}