using System;
using System.Collections.Generic;
using Core.Entities.HR;
using Core.Enumerations;

namespace API.Dtos
{
    public class CandidateToAddDto
    {
        public int ApplicationNo { get; set; }
        public DateTime ApplicationDated { get; set; }
        
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FamilyName { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string PPNo { get; set; }
        public bool ECNR {get; set; }
        public string AadharNo { get; set; }
        public DateTime DOB { get; set; }

        // public int ProfessionId {get; set; }
        public List<CandidateProfessionDto> FormArrayCategories {get; set;}
        public string MobileNo {get; set; }
        public string email {get; set; }

        public List<CandidateAddressDto> FormArrayAddresses {get; set;}
      /*
        public string AddressType {get; set; }
        public string Address1 {get; set; }
        public string Address2 {get; set; }
        public string City {get; set; }
        public string PIN {get; set; }
        public string State {get; set; }
        public string District {get; set; }
        public string Country {get; set;}
    */
    }

    public class CandidateProfessionDto
    {
        public int ProfessionId{get; set;}
    }

    public class CandidateAddressDto
    {
        public string AddressType {get; set; }
        public string Address1 {get; set; }
        public string Address2 {get; set; }
        public string City {get; set; }
        public string PIN {get; set; }
        public string State {get; set; }
        public string District {get; set; }
        public string Country {get; set;}
    }
}