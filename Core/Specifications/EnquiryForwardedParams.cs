using System;

namespace Core.Specifications
{
    public class EnquiryForwardedParams
    {
        private const int MaxmPageSize = 50;
        
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxmPageSize) ? MaxmPageSize : value;
        }

        public int? CustomerId { get; set; }
        public bool includeItems {get; set; }
        public bool includeCustomers {get; set; }
        public string Sort { get; set; }

        private string _search;
        public string Search 
        { 
            get => _search; 
            set => value.ToLower(); 
        }

        public DateTime? Date1 {get; set;}
        public DateTime? Date2 {get; set; }

    }
}