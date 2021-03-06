namespace Core.Specifications
{
    public class CategorySpecsParams
    {

        public int? IndustryTypeId { get; set; }
        public int? SkillLevelId { get; set; }

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
            set => _search = value.ToLower(); 
        }
        
    }
}