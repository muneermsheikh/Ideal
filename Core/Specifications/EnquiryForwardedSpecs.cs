using System;
using System.Linq.Expressions;
using Core.Entities.Admin;

namespace Core.Specifications
{
    public class EnquiryForwardedSpecs : BaseSpecification<EnquiryForwarded>
    {
        public EnquiryForwardedSpecs(EnquiryForwardedParams enqParams) 
            : base(x => ( 
                (string.IsNullOrEmpty(enqParams.Search) || 
                    x.Customer.CustomerName.ToLower().Contains(enqParams.Search)) &&
                (!enqParams.Date1.HasValue && !enqParams.Date2.HasValue || 
                    x.ForwardedOn <= enqParams.Date2 && x.ForwardedOn >= enqParams.Date1) &&
                (!enqParams.CustomerId.HasValue || 
                    x.CustomerId == enqParams.CustomerId)))
        {
            AddInclude(x => x.EnquiryItemsForwarded);
            AddOrderByDescending(x => x.ForwardedOn);
        }
        public EnquiryForwardedSpecs(int enquiryId) : base(x => x.EnquiryId == enquiryId)
        {
            AddInclude(x => x.EnquiryItemsForwarded);
            AddOrderByDescending(x => x.ForwardedOn);
        }

        public EnquiryForwardedSpecs(int customerId, string dummy) : base(x => x.CustomerId == customerId)
        {
            AddInclude(x => x.EnquiryItemsForwarded);
            AddOrderBy(x => x.EnquiryId);
            AddOrderByDescending(x => x.ForwardedOn);
        }

    }
}