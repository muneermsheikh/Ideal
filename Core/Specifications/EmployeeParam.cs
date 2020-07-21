using System;

namespace Core.Specifications
{
    public class EmployeeParam
    {
        public int? Id {get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FamilyName { get; set; }
        public string Gender { get; set; }
        public string Designation { get; set; }
        public DateTimeOffset? DOJ { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool? IsInEmployment { get; set; }

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