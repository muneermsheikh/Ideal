using System;
using Core.Enumerations;

namespace Core.Entities.Admin
{
    public class ContractReviewItem: BaseEntity
    {
        public ContractReviewItem()
        {
        }

        public ContractReviewItem(int enquiryItemId, int enquiryId)
        {
            EnquiryItemId = enquiryItemId;
            EnquiryId = enquiryId;
        }

        public int EnquiryId { get; set; }
        public int EnquiryItemId { get; set; }
        public string TechnicallyFeasible { get; set; }="f";
        public string CommerciallyFeasible { get; set; }="f";
        public string LogisticallyFeasible { get; set; }="f";
        public string VisaAvailable { get; set; }="f";
        public string DocumentationWillBeAvailable { get; set; }="f";
        public string HistoricalStatusAvailable { get; set; }="f";
        public string SalaryOfferedFeasible { get; set; }="f";
        public string ServiceChargesInINR { get; set; }
        public string FeeFromClientCurrency { get; set; }
        public int FeeFromClient { get; set; } 
        public string Status { get; set; } = "NotReviewed";
        public DateTime ReviewedOn { get; set; } = DateTime.Now;
        public int ReviewedBy { get; set; } = 0;

    }
}