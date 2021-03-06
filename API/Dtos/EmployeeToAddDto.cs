using System;
using System.Collections.Generic;
using Core.Entities.Masters;

namespace API.Dtos
{
    public class EmployeeToAddDto
    {
        public string Gender {get; set; }
        public string FirstName { get; set; }
        public string SecondName {get; set; }
        public string FamilyName {get; set; }
        public string KnownAs {get; set; }
    /*    
        public string Address1 {get; set; }
        public string Address2 {get; set; }
        public string City {get; set; }
        public string PIN {get; set; }
        public string District {get; set; }
        public string State {get; set; }
    */    
        public string Mobile {get; set;}
        public string Email {get; set; }
        public string AadharNo {get; set; }
        public string PassportNo {get; set; }

        public string Designation {get; set; }
        public DateTime DateOfBirth {get; set;}
        public DateTime DateOfJoining {get; set; }
        public virtual List<EmployeeAddress> Addresses { get; set; }
    }
}