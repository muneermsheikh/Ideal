using System;
using Core.Enumerations;

namespace Core.Specifications
{
    public class EnquiryParams
    {
        public int? Id {get; set; }

        private const int MaxmPageSize = 50;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxmPageSize) ? MaxmPageSize : value;
        }
        public string Sort { get; set; }
        public string CustomerName 
        {   
            get => _customerName; 
            set => _customerName = value.ToLower(); 
        }
        public DateTime? EnquiryDate {get; set; }
        public string EnquiryNo {get; set; }
        private string _customerName;
        public string EnquiryStatus;
        public string ReviewStatus;
        
    }
}