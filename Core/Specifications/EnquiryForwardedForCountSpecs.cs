using Core.Entities.Admin;

namespace Core.Specifications
{
    public class EnquiryForwardedForCountSpecs : BaseSpecification<EnquiryForwarded>
    {
        public EnquiryForwardedForCountSpecs(EnquiryForwardedParams enqParams) 
            : base(x => ( 
                (string.IsNullOrEmpty(enqParams.Search) || 
                    x.Customer.CustomerName.ToLower().Contains(enqParams.Search)) &&
                (!enqParams.Date1.HasValue && !enqParams.Date2.HasValue || 
                    x.ForwardedOn <= enqParams.Date2 && x.ForwardedOn >= enqParams.Date1) &&
                (!enqParams.CustomerId.HasValue || 
                    x.CustomerId == enqParams.CustomerId)))
        {
        }
        public EnquiryForwardedForCountSpecs(int enquiryId) : base(x => x.EnquiryId == enquiryId)
        {
            AddOrderByDescending(x => x.ForwardedOn);
        }
    }
}