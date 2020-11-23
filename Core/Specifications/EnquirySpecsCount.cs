using System;
using System.Linq.Expressions;
using Core.Entities.EnquiryAggregate;

namespace Core.Specifications
{
    public class EnquirySpecsCount : BaseSpecification<Enquiry>
    {
        public EnquirySpecsCount(EnquiryParams eParam) 
             :base ( x => (
                (!eParam.Id.HasValue || x.Id == eParam.Id) &&
                (!eParam.EnquiryDate.HasValue || DateTime.Compare(
                    x.EnquiryDate.Date, eParam.EnquiryDate.Value.Date)==0) &&
                (!string.IsNullOrEmpty(eParam.EnquiryNo) || x.EnquiryNo == eParam.EnquiryNo) &&
                (!string.IsNullOrEmpty(eParam.EnquiryStatus) || x.EnquiryStatus == eParam.EnquiryStatus) &&
                (!string.IsNullOrEmpty(eParam.ReviewStatus) || x.ReviewStatus == eParam.ReviewStatus))
             )
        {
        }

         public EnquirySpecsCount(int customerId, string dummy): 
            base (o => o.CustomerId == customerId)
        {
        }
    
        public EnquirySpecsCount(int enquiryId): 
            base (o => o.Id == enquiryId)
        {
        }
    }
}