using System;
using System.Linq.Expressions;
using Core.Entities.Admin;
using Core.Enumerations;

namespace Core.Specifications
{
    public class CVForwardSpecs : BaseSpecification<CVForward>
    {
        public CVForwardSpecs(CVForwardParam param)
            :  base( x => 
            (
                (!param.CustomerId.HasValue || x.CustomerId == param.CustomerId) &&
                (!param.DateFrom.HasValue && !param.DateUpto.HasValue|| 
                    (DateTime.Compare(x.DateForwarded.Date, (DateTime)param.DateFrom) >= 0 &&
                    DateTime.Compare(x.DateForwarded.Date, (DateTime)param.DateUpto) <= 0))
            ))
        {            
            
            AddInclude(x => x.CVForwardItems);

            ApplyPaging(param.PageSize * (param.PageIndex-1), param.PageSize);

                if (!string.IsNullOrEmpty(param.Sort))
                {
                    switch (param.Sort.ToLower())
                    {
                        case "customername":
                            AddOrderBy(x => x.Customer.CustomerName);
                            break;
                        case "customernamedesc":
                            AddOrderByDescending(x => x.Customer.CustomerName);
                            break;

                        case "dateforwardeddesc":
                            AddOrderByDescending(x => x.DateForwarded);
                            break;
                        default:
                            AddOrderBy(x => x.DateForwarded);
                            break;
                }
            }
        }
    }
}