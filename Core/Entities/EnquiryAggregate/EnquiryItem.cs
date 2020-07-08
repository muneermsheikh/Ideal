using System;
using Core.Entities.Admin;
using Core.Entities.Masters;
using Core.Enumerations;

namespace Core.Entities.EnquiryAggregate
{
    public class EnquiryItem : BaseEntity
    {
        public EnquiryItem()
        {
        }

        
        public EnquiryItem(int quantity)
        {
            // ItemOrdered = itemOrdered;
            Quantity = quantity;
            Status = enumItemReviewStatus.NotReviewed;
        }
        

        // public CategoryItemOrdered ItemOrdered { get; set; }
        public int Price { get; set; }

        public int EnquiryId { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public int MinCVs { get; set; }
        public int MaxCVs { get; set; }
        public bool ECNR { get; set; }
        public bool RequireAssessment { get; set; }
        public virtual Employee HRExecutive { get; set; }
        public virtual Employee HRSupervisor { get; set; }
        public virtual Employee HRManager { get; set; }
        public Category Category {get; set; }
        public virtual ContractReviewItem ContractReviewItem { get; set; }
        public string SCFromCandidate { get; set; }
        public string FeeFromClientCurrency { get; set; }
        public int FeeFromClient { get; set; }
        public enumItemReviewStatus Status { get; set; } = enumItemReviewStatus.NotReviewed;
        public DateTime CompleteBy { get; set; }
        public virtual JobDesc JobDesc { get; set; }
        public int JobDescId { get; set; }
        public virtual Remuneration Remuneration { get; set; }
        public int RemunerationId { get; set; }
    }
}