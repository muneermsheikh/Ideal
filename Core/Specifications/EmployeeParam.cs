using System;

namespace Core.Specifications
{
    public class EmployeeParam
    {
        public int? Id {get; set; }
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

        public string FirstName 
        {   
            get => _firstName; 
            set => _firstName = value.ToLower(); 
        }
        public string FamilyName 
        { 
            get => _familyName; 
            set => _familyName = value.ToLower(); 
        }
        public string Gender 
        { 
            get => _gender; 
            set => _gender = value.ToLower(); 
        }
        public DateTime? DOJ {get; set; }

        //04066864700 - aig 
        public string Designation
        { 
            get => _designation; 
            set => _designation = value.ToLower(); 
        }
        public string Email
        { 
            get => _email; 
            set => _email = value.ToLower(); 
        }
        private string _firstName;
        private string _familyName;
        private string _designation;
        private string _gender;
        private string _email;
        private DateTime _doj;
        

    }
}