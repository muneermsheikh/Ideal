using System;
using Core.Enumerations;

namespace Core.Specifications
{
    public class CVRefParam
    {
        private const int MaxmPageSize = 50;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxmPageSize) ? MaxmPageSize : value;
        }

        public int? Id { get; set; }
        public int? EnquiryItemId {get; set;}
        public int? EnquiryId {get; set;}
        public string CustomerName {get; set;}
        public DateTime? ForwardedFrom {get; set;}
        public DateTime? ForwardedUpto {get; set;}
        public string Sort { get; set; }

        private string _search;
        public string Search 
        { 
            get => _search; 
            set => value.ToLower(); 
        }
    }
}