using System;
using Core.Enumerations;

namespace API.Dtos
{
    public class ReviewItemToUpdateDto
    {
        public int Id {get; set; }
        public int EnquiryId { get; set; }
        public int EnquiryItemId { get; set; }
        public bool TechnicallyFeasible { get; set; }
        public bool CommerciallyFeasible { get; set; }
        public bool LogisticallyFeasible { get; set; }
        public bool VisaAvailable { get; set; }
        public bool DocumentationWillBeAvailable { get; set; }
        public bool HistoricalStatusAvailable { get; set; }
        public bool SalaryOfferedFeasible { get; set; }
        public string ServiceChargesInINR { get; set; }
        public string FeeFromClientCurrency { get; set; }
        public int FeeFromClient { get; set; } 
        public enumItemReviewStatus Status { get; set; }
        public DateTime ReviewedOn { get; set; }
        public int ReviewedBy { get; set; } 
    }
}