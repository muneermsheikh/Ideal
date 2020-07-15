using Core.Entities.Admin;

namespace Core.Specifications
{
    public class EnqForwardedWithFiltersForCountSpec : BaseSpecification<EnquiryForwarded>
    {
        public EnqForwardedWithFiltersForCountSpec(EnqForwardSpecParams enqParams) 
            : base(x => ( 
                (string.IsNullOrEmpty(enqParams.Search) || 
                    x.Customer.CustomerName.ToLower().Contains(enqParams.Search)) &&
                (!enqParams.EnquiryId.HasValue || 
                    x.EnquiryId == enqParams.EnquiryId) &&
                (!enqParams.EnquiryItemId.HasValue || 
                    x.EnquiryItemId == enqParams.EnquiryItemId) &&
                (!enqParams.CustomerId.HasValue || 
                    x.CustomerId == enqParams.CustomerId) &&
                (!enqParams.CustomerOfficialId.HasValue || 
                    x.CustomerOfficialId == enqParams.CustomerOfficialId) &&
                (string.IsNullOrEmpty(enqParams.ModeOfSending) || 
                    x.ForwardedByMode == enqParams.ModeOfSending)))
        {
        }
    }
}