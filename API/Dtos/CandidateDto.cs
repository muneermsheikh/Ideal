using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Masters;
using Core.Enumerations;

namespace API.Dtos
{
    public class CandidateDto
    {
        
        public CandidateDto(int id, int applicationNo, DateTime applicationDated, string gender, 
            string fullName, int age, string pPNo, string aadharNo, string city, string candidateStatus, 
            List<CategoryNameDto> candidateCategories)
        {
            Id = id;
            ApplicationNo = applicationNo;
            ApplicationDated = applicationDated;
            Gender = gender;
            FullName = fullName;
            Age = age;
            PPNo = pPNo;
            AadharNo = aadharNo;
            City = city;
            CandidateStatus = candidateStatus;
            CandidateCategories = candidateCategories;
        }
        

        public int Id {get; set;}
        public int ApplicationNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime ApplicationDated { get; set; }
        public string Gender { get; set; }
        public string FullName { get; set; }
        public string KnownAs {get; set;}
        public int Age {get; set; }
        public string PPNo { get; set; }
        public string AadharNo {get; set;}
        public bool ECNR {get; set; }
        public string City { get; set; }
        public string ReferredBy {get; set;}
        
        public string CandidateStatus { get; set; }
        public string CandidateString {get; set; }
        public virtual List<CategoryNameDto> CandidateCategories { get; set; }
        
    }
}