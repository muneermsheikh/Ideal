using System;
using System.Collections.Generic;
using Core.Entities.HR;
using Core.Entities.Masters;
using Core.Enumerations;

namespace Core.Specifications
{
    public class SelDecisionParams
    {
        public int? Id {get; set; }
        public int? CVRefId { get; set; }
        public int? ApplicationNo {get; set;}
        public int? EnquiryId {get; set;}
        public int? EnquiryItemId {get; set;}
        public DateTime? SelDateFrom {get; set;}
        public DateTime? SelDateUpto {get; set;}
        public enumSelectionResult? SelectionResult { get; set; } 

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