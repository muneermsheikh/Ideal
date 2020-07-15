using System;
using Core.Enumerations;

namespace Core.Entities.Admin
{
    public class ContractReviewItem: BaseEntity
    {
        public ContractReviewItem(int enquiryItemId, int enquiryId)
        {
            EnquiryItemId = enquiryItemId;
            EnquiryId = enquiryId;
        }

        public int EnquiryId { get; set; }
        public int EnquiryItemId { get; set; }
        public bool TechnicallyFeasible { get; set; }=false;
        public bool CommerciallyFeasible { get; set; }=false;
        public bool LogisticallyFeasible { get; set; }=false;
        public bool VisaAvailable { get; set; }=false;
        public bool DocumentationWillBeAvailable { get; set; }=false;
        public bool HistoricalStatusAvailable { get; set; }=false;
        public bool SalaryOfferedFeasible { get; set; }=false;
        public string ServiceChargesInINR { get; set; }
        public string FeeFromClientCurrency { get; set; }
        public int FeeFromClient { get; set; } 
        public enumItemReviewStatus Status { get; set; } = enumItemReviewStatus.NotReviewed ;
        public DateTimeOffset ReviewedOn { get; set; } = DateTimeOffset.Now;
        public int? ReviewedBy { get; set; }

    }
}