using Core.Enumerations;

namespace Core.Specifications
{
    public class CustomerSpecParams
    {
        private const int MaxmPageSize = 50;
        
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxmPageSize) ? MaxmPageSize : value;
        }

        public int? CustomerId {get; set; }
        public string CustomerType {get; set; }
        public string CustomerStatus {get; set; }
        public string City {get; set; }
        public string Email {get; set; }
        public bool IncludeOfficial {get; set;}=true;
        public bool IncludeIndustryTypes {get; set;}=true;
        public string Sort { get; set; }

        private string _search;

        public string Search 
        { 
            get => _search; 
            set => _search = value.ToLower(); 
        }
    }
}