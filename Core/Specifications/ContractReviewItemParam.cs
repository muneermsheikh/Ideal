using System;
using Core.Enumerations;

namespace Core.Specifications
{
    public class ContractReviewItemParam
    {
        public int? EnquiryId { get; set; }
        public int? EnquiryItemId { get; set; }
        public string TechnicallyFeasible { get; set; }="f";
        public string CommerciallyFeasible { get; set; }="f";
        public string LogisticallyFeasible { get; set; }="f";
        public string VisaAvailable { get; set; }="f";
        public string DocumentationWillBeAvailable { get; set; }="f";
        public string HistoricalStatusAvailable { get; set; }="f";
        public string SalaryOfferedFeasible { get; set; }="f";
        public string ServiceChargesInINR { get; set; }
        public string FeeFromClientCurrency { get; set; }
        public int? FeeFromClient { get; set; } 
        public string Status { get; set; }
        public DateTime? ReviewedOn { get; set; } 
        public int? ReviewedBy { get; set; }

        private const int MaxmPageSize = 50;
        
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxmPageSize) ? MaxmPageSize : value;
        }
         public string Sort { get; set; }

        private string _search;

        public string Search 
        { 
            get => _search; 
            set => value.ToLower(); 
        }
    }
}