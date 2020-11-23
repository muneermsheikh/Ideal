using System;
using System.Linq.Expressions;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;

namespace Core.Specifications
{
    public class EnquiryItemSpecsCount : BaseSpecification<EnquiryItem>
    {
        public EnquiryItemSpecsCount(int enquiryId, string reviewStatus) 
            : base(x => x.EnquiryId == enquiryId &&  x.ReviewStatus == reviewStatus)
        {
        }
    }
}