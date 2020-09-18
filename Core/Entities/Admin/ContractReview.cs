using System;
using Core.Enumerations;

namespace Core.Entities.Admin
{
    public class ContractReview: BaseEntity
    {
        public ContractReview()
        {
        }

        public ContractReview(bool readyToFinalize, int enquiryId, int reviewedBy, enumEnquiryReviewStatus reviewStatus)
        {
            ReadyToFinalize = readyToFinalize;
            EnquiryId = enquiryId;
            ReviewedBy = reviewedBy;
            ReviewedOn = DateTime.Now;
            ReviewStatus = reviewStatus;
        }

        public bool ReadyToFinalize { get; set; }       //set to true when all contract reviewitems are reviewed
        public int EnquiryId { get; set; }
        public int ReviewedBy { get; set; }
        public DateTime ReviewedOn { get; set; } = DateTime.Now;
        public enumEnquiryReviewStatus ReviewStatus { get; set; } = enumEnquiryReviewStatus.NotReviewed;
    }
}