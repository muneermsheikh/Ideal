using System;
using System.Linq.Expressions;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;

namespace Core.Specifications
{
    public class EnquirySpecs : BaseSpecification<Enquiry>
    {
       /* public EnquirySpecs(string email): base (o => o.BuyerEmail == email)
        {
            AddInclude(o => o.EnquiryItems);
            AddOrderByDescending(o => o.EnquiryDate);
        }
        */
        public EnquirySpecs (EnquiryParams eParam)
            :base ( x => (
                (string.IsNullOrEmpty(eParam.CustomerName) || 
                        x.Customer.CustomerName.ToLower().Contains(eParam.CustomerName)) &&
                    (!eParam.Id.HasValue || x.Id == eParam.Id) &&
                    (!eParam.EnquiryNo.HasValue || x.EnquiryNo == eParam.EnquiryNo) &&
                    (!eParam.EnquiryDate.HasValue || DateTime.Compare(
                        x.EnquiryDate.Date, eParam.EnquiryDate.Value.Date)==0) &&
                    (!eParam.status.HasValue || x.EnquiryReviewStatusId == eParam.status))
            )
        {
            AddInclude(o => o.Customer);
            AddInclude(o => o.EnquiryItems);
            AddOrderBy(o => o.EnquiryNo);
        }
        public EnquirySpecs(int customerId, string dummy): 
            base (o => o.CustomerId == customerId)
        {
            AddInclude(o => o.Customer);
            AddInclude(o => o.EnquiryItems);
            AddOrderBy(o => o.EnquiryNo);
        }
    
        public EnquirySpecs(int enquiryId, enumEnquiryReviewStatus enquiryStatus, bool includeCustomer, bool includeItems) 
            :base (o => (o.Id == enquiryId && o.EnquiryReviewStatusId==enquiryStatus))
        {
            if (includeCustomer) AddInclude(o => o.Customer);
            if (includeItems) AddInclude(o => o.EnquiryItems);
        }

    }
}