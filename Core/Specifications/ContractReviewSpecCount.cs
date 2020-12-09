using System;
using Core.Entities.Admin;
using Core.Entities.HR;

namespace Core.Specifications
{
    public class ContractReviewSpecCount: BaseSpecification<ContractReview>
    {
        public ContractReviewSpecCount(ContractReviewParam cParam) 
           : base(x => (
                (!cParam.EnquiryId.HasValue || x.EnquiryId == cParam.EnquiryId) &&
                (!cParam.CustomerId.HasValue || x.CustomerId == cParam.CustomerId) &&
                (!cParam.ReviewedBy.HasValue || x.ReviewedBy == cParam.ReviewedBy)
            ))
        {
        }

        public ContractReviewSpecCount(int enquiryId): base(x => x.EnquiryId == enquiryId)
        {
        }
        
    }
}