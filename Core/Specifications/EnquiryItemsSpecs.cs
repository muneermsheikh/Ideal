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

    }
}