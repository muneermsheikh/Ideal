using System;
using Core.Enumerations;

namespace Core.Specifications
{
    public class TaskSpecParams
    {
        private const int MaxmPageSize = 50;
        
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxmPageSize) ? MaxmPageSize : value;
        }

        public int? TaskId {get; set; }
        public int? TaskItemId { get; set; }
        public int? OwnerId {get; set; }
        public int? AssignedToId {get; set; }
        public string TaskStatus {get; set; }
        public string TaskType {get; set; }
        public DateTime? DateRangeFrom {get; set; }
        public DateTime? DateRangeUpto {get; set; }
        public bool IncludeItems {get; set; } = false;
        public string Sort { get; set; }

        private string _search;

        public string Search 
        { 
            get => _search; 
            set => value.ToLower(); 
        }
    }
}