using System;
using System.Linq.Expressions;
using Core.Entities.EnquiryAggregate;

namespace Core.Specifications
{
    public class EnquirySpecsCount : BaseSpecification<Enquiry>
    {
        public EnquirySpecsCount(EnquiryParams eParam) 
             :base ( x => (
                (string.IsNullOrEmpty(eParam.CustomerName) || 
                        x.Customer.CustomerName.ToLower().Contains(eParam.CustomerName)) &&
                    (!eParam.Id.HasValue || x.Id == eParam.Id) &&
                    (!eParam.EnquiryNo.HasValue || x.EnquiryNo == eParam.EnquiryNo) &&
                    (!eParam.EnquiryDate.HasValue || DateTime.Compare(
                        x.EnquiryDate.Date, eParam.EnquiryDate.Value.Date)==0) &&
                    (!eParam.status.HasValue || x.EnquiryStatus == eParam.status))
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