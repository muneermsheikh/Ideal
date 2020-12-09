using System;
using System.Linq.Expressions;
using Core.Entities.Admin;

namespace Core.Specifications
{
    public class ContractReviewSpec : BaseSpecification<ContractReview>
    {
        public ContractReviewSpec(ContractReviewParam cParam) 
            : base(x => (
                (!cParam.EnquiryId.HasValue || x.EnquiryId == cParam.EnquiryId) &&
                (!cParam.CustomerId.HasValue || x.CustomerId == cParam.CustomerId) &&
                (!cParam.ReviewedBy.HasValue || x.ReviewedBy == cParam.ReviewedBy)
            ))
        {
            AddOrderByDescending(x => x.EnquiryNo);
            AddInclude(x => x.ContractReviewItems);
        }

        public ContractReviewSpec(int enquiryId): base(x => x.EnquiryId == enquiryId)
        {
        }
    }
    
}