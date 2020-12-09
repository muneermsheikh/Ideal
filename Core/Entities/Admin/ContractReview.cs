using System;
using System.Collections.Generic;
using Core.Enumerations;

namespace Core.Entities.Admin
{
    public class ContractReview: BaseEntity
    {
        public ContractReview()
        {
        }

        public ContractReview(bool readyToFinalize, int enquiryId, int reviewedBy, string reviewStatus)
        {
            ReadyToFinalize = readyToFinalize;
            EnquiryId = enquiryId;
            ReviewedBy = reviewedBy;
            ReviewedOn = DateTime.Now;
            ReviewStatus = reviewStatus;
        }

        public bool ReadyToFinalize { get; set; }       //set to true when all contract reviewitems are reviewed
        public int EnquiryId { get; set; }
        public int EnquiryNo {get; set; }
        public DateTime? EnquiryDate {get; set;}
        public int CustomerId {get; set;}
        public string CustomerName {get; set;}
        public int? ReviewedBy { get; set; }
        public DateTime ReviewedOn { get; set; } = DateTime.Now;
        public List<ContractReviewItem> ContractReviewItems {get; set; }
        public string ReviewStatus { get; set; } = "NotReviewed";
    }
}