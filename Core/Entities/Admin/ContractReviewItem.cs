using System;
using Core.Enumerations;

namespace Core.Entities.Admin
{
    public class ContractReviewItem: BaseEntity
    {
        public ContractReviewItem()
        {
        }
        public ContractReviewItem(int enquiryItemId, int enquiryId, int quantity, string categoryName)
        {
            EnquiryItemId = enquiryItemId;
            EnquiryId = enquiryId;
            Quantity = quantity;
            CategoryName = categoryName;
        }

        public ContractReviewItem(int enquiryItemId, int enquiryId, int contractReviewId, int quantity, string categoryName)
        {
            EnquiryItemId = enquiryItemId;
            EnquiryId = enquiryId;
            ContractReviewId = contractReviewId;
            Quantity = quantity;
            CategoryName = categoryName;
        }

        public int ContractReviewId {get; set;}
        public int EnquiryId { get; set; }
        public int EnquiryItemId { get; set; }
        public string CategoryName {get; set;}
        public int Quantity {get; set; }
        public bool TechnicallyFeasible { get; set; }=false;
        public bool CommerciallyFeasible { get; set; }=false;
        public bool LogisticallyFeasible { get; set; }=false;
        public bool VisaAvailable { get; set; }=false;
        public bool DocumentationWillBeAvailable { get; set; }=false; 
        public bool HistoricalStatusAvailable { get; set; }=false;
        public bool SalaryOfferedFeasible { get; set; }=false;
        public string ServiceChargesInINR { get; set; }="";
        public string FeeFromClientCurrency { get; set; }="";
        public int FeeFromClient { get; set; } =0;
        public string Status { get; set; } = "NotReviewed";
        public DateTime ReviewedOn { get; set; } = DateTime.Now;
        public int ReviewedBy { get; set; } = 0;

    }
}