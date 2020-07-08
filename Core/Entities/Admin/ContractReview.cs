using System;
using Core.Enumerations;

namespace Core.Entities.Admin
{
    public class ContractReview: BaseEntity
    {
        public ContractReview()
        {
            ReviewedOn = DateTime.Now;
            ReviewStatus = enumReviewStatus.NotReviewed;
        }

        public int EnquiryId { get; set; }
        public int ReviewedBy { get; set; }
        public DateTime ReviewedOn { get; set; }
        public enumReviewStatus ReviewStatus { get; set; }
    }
}