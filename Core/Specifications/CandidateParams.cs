using System;
using System.Collections.Generic;
using Core.Entities.HR;
using Core.Entities.Masters;
using Core.Enumerations;

namespace Core.Specifications
{
    public class CandidateParams
    {
        public int? Id {get; set; }
        public int? ApplicationNo { get; set; }
        public DateTime? ApplicationDated { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public string PPNo { get; set; }
        public bool includeAddress {get; set; }
        public bool includeCategories {get; set; }
        public enumCandidateStatus? CandidateStatus { get; set; } = enumCandidateStatus.Available;
        public string FullName {get {return FirstName + ", " + FamilyName;} }


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