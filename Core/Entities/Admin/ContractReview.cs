using System;
using Core.Enumerations;

namespace Core.Entities.Admin
{
    public class ContractReview: BaseEntity
    {
        public ContractReview()
        {
        }

        public ContractReview(bool readyToFinalize, int enquiryId, int reviewedBy, enumReviewStatus reviewStatus)
        {
            ReadyToFinalize = readyToFinalize;
            EnquiryId = enquiryId;
            ReviewedBy = reviewedBy;
            ReviewedOn = DateTimeOffset.Now;
            ReviewStatus = reviewStatus;
        }

        public bool ReadyToFinalize { get; set; }       //set to true when all contract reviewitems are reviewed
        public int EnquiryId { get; set; }
        public int ReviewedBy { get; set; }
        public DateTimeOffset ReviewedOn { get; set; } = DateTimeOffset.Now;
        public enumReviewStatus ReviewStatus { get; set; } = enumReviewStatus.NotReviewed;
    }
}