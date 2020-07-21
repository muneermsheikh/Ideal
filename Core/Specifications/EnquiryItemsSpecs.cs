using System;
using System.Linq.Expressions;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;

namespace Core.Specifications
{
    public class EnquiryItemsSpecs : BaseSpecification<EnquiryItem>
    {
        public EnquiryItemsSpecs()
        {
        }

        public EnquiryItemsSpecs(int enquiryId, enumItemReviewStatus reviewStatus) 
            : base(x => x.EnquiryId == enquiryId &&  x.Status == reviewStatus)
        {
        }

        public EnquiryItemsSpecs(int enquiryItemId, enumItemReviewStatus itemStatus, 
            bool HRSupAssigned) 
            : base(x => (x.Id == enquiryItemId && x.Status == itemStatus) &&
                (x.AssessingSupId.HasValue) )
        {
        }
        
        public EnquiryItemsSpecs(int enquiryItemId) 
            : base(x => x.Id == enquiryItemId)
        {
        }


    }
}