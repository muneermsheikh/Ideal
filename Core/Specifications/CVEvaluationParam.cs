using System;
using Core.Enumerations;

namespace Core.Specifications
{
    public class CVEvaluationParam
    {
        private const int MaxmPageSize = 50;
       
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxmPageSize) ? MaxmPageSize : value;
        }
        public int Id { get; set; }
        public int EnquiryItemId {get; set; }
        public int? ApplicationNo{get; set; }
        public int? HRExecutiveId { get; set; }
        public DateTime? SubmittedByHRExecOn { get; set; }
        public int? HRSupervisorId { get; set; }
        public string HRSupReviewResult { get; set; }
        public DateTime? ReviewedByHRSupOn { get; set; }
        public int? HRManagerId { get; set; }
        public DateTime? ReviewedByHRMOn { get; set; }
        public string HRMReviewResult { get; set; }
        
        public string Sort { get; set; }

        private string _search;

        public string Search 
        { 
            get => _search; 
            set => value.ToLower(); 
        }
    }
}