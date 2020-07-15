using Core.Enumerations;

namespace Core.Specifications
{
    public class EnqItemsSpecParams
    {
        private const int MaxmPageSize = 50;
        
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 2;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxmPageSize) ? MaxmPageSize : value;
        }

        public int? EnquiryId {get; set; }
        public int? EnquiryItemId { get; set; }
        public enumItemReviewStatus? ItemStatus {get; set; }
        public string Sort { get; set; }

        private string _search;

        public string Search 
        { 
            get => _search; 
            set => value.ToLower(); 
        }
    }
}