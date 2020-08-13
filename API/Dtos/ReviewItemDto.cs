using System;

namespace API.Dtos
{
    public class ReviewItemDto
    {
        public int EnquiryId { get; set; }
        public string EnquiryNoAndDate {get; set;}
        public string CustomerName {get; set;}
        public string CategoryName {get; set; }
        
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
        public string Status { get; set; } 
        public DateTime ReviewedOn { get; set; }
        public string ReviewedByName { get; set; }

    }
}