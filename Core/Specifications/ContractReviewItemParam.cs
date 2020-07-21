using System;
using Core.Enumerations;

namespace Core.Specifications
{
    public class ContractReviewItemParam
    {
        public int? EnquiryId { get; set; }
        public int? EnquiryItemId { get; set; }
        public bool? TechnicallyFeasible { get; set; }=false;
        public bool? CommerciallyFeasible { get; set; }=false;
        public bool? LogisticallyFeasible { get; set; }=false;
        public bool? VisaAvailable { get; set; }=false;
        public bool? DocumentationWillBeAvailable { get; set; }=false;
        public bool? HistoricalStatusAvailable { get; set; }=false;
        public bool? SalaryOfferedFeasible { get; set; }=false;
        public string ServiceChargesInINR { get; set; }
        public string FeeFromClientCurrency { get; set; }
        public int? FeeFromClient { get; set; } 
        public enumItemReviewStatus? Status { get; set; }
        public DateTimeOffset? ReviewedOn { get; set; } 
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