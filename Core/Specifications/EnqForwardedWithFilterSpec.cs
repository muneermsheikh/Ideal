using System;
using System.Linq.Expressions;
using Core.Entities.Admin;

namespace Core.Specifications
{
    public class EnqForwardedWithFilterSpec : BaseSpecification<EnquiryForwarded>
    {
        public EnqForwardedWithFilterSpec(EnqForwardSpecParams enqParams)
            : base( x => (
                (string.IsNullOrEmpty(enqParams.Search) || 
                    x.Customer.CustomerName.ToLower().Contains(enqParams.Search)) &&
                (!enqParams.EnquiryId.HasValue || x.EnquiryId == enqParams.EnquiryId) &&
                (!enqParams.CustomerId.HasValue || x.CustomerId == enqParams.CustomerId) &&
                (!enqParams.CustomerOfficialId.HasValue || x.CustomerOfficialId == enqParams.CustomerOfficialId) &&
                (string.IsNullOrEmpty(enqParams.ModeOfSending) || x.ForwardedByMode == enqParams.ModeOfSending)))
        {
            AddInclude(x => x.Customer);
            AddOrderByDescending( x => x.ForwardedOn);

            ApplyPaging(enqParams.PageSize, enqParams.PageSize * (enqParams.PageIndex-1));

            if (!string.IsNullOrEmpty(enqParams.Sort))
            {
                switch (enqParams.Sort)
                { 
                    case "CustomerNameAsc":
                        AddOrderBy(x => x.Customer.CustomerName);
                        break;
                     case "CustomerNameDesc":
                        AddOrderByDescending(x => x.Customer.CustomerName);
                        break;
                    case "ForwardedOnAsc":
                        AddOrderBy(x => x.ForwardedOn);
                        break;
                    case "ForwardedOnDesc":
                        AddOrderByDescending(x => x.ForwardedOn);
                        break;
                    case "EnquiryIdAsc":
                        AddOrderBy(x => x.EnquiryId);
                        break;
                    case "EnquiryIdDesc":
                        AddOrderByDescending(x => x.EnquiryId);
                        break;
                    default:
                        AddOrderByDescending(x => x.ForwardedOn);
                        break;
                }
            }
        }
        public EnqForwardedWithFilterSpec(int enquiryId)
            : base( x => x.EnquiryId == enquiryId)
        {
            AddInclude(x => x.Customer);
            AddOrderByDescending( x => x.ForwardedOn);
        }
    }
}