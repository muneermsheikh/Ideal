using System;
using System.Collections.Generic;
using Core.Entities.Masters;
using Core.Enumerations;

namespace API.Dtos
{
    public class CandidateDto
    {
        public int Id {get; set;}
        public int ApplicationNo { get; set; }
        public DateTime ApplicationDated { get; set; }
        public string Gender { get; set; }
        public string FullName { get; set; }
        public int Age {get; set; }
        public string PPNo { get; set; }
        public string AadharNo { get; set; }
        public string City { get; set; }
        
        public List<CategoryNameDto> CandidateCategories { get; set; }
        //public enumCandidateStatus CandidateStatus { get; set; } 
        public string CandidateStatus { get; set; } 
        
    }
}