namespace Core.Specifications
{
    public class AssessmentQFromBankParams
    {
        private const int MaxmPageSize = 50;
        
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxmPageSize) ? MaxmPageSize : value;
        }

        public int? CategoryId {get; set; }
        public bool IsStandardQuestion { get; set; }
        public string AssessmentParameter { get; set; }
        private string _search;
        public string Search 
        { 
            get => _search; 
            set => value.ToLower(); 
        }
    }
}