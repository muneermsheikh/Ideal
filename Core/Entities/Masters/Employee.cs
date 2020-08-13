using System;
using System.Collections.Generic;
using Core.Entities.HR;

namespace Core.Entities.Masters
{
    public class Employee: BaseEntity
    {
        public Employee()
        {
        }

        public Employee(string firstName, string secondName,  string familyName, string knownAs, 
            string gender, DateTime dOB, string pPNo, string aadharNo, string mobile, string email,
            List<EmployeeAddress> employeeAddresses, string designation, DateTime dOJ)
        {
            FirstName=firstName;
            SecondName=secondName;
            FamilyName=familyName;
            KnownAs=knownAs;
            Gender=gender;
            DOB=dOB;
            Designation = designation;
            DOJ = dOJ;
            PPNo = pPNo;
            AadharNo = aadharNo;
            Mobile = mobile;
            Email = email;
            Addresses = employeeAddresses;
        }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FamilyName { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string PPNo { get; set; }
        public string AadharNo { get; set; }
        public DateTime? DOB { get; set; }

        public string Designation { get; set; }
        public DateTime DOJ { get; set; }
        public string Email {get; set; }
        public string Mobile {get; set; }
        public virtual List<EmployeeAddress> Addresses { get; set; }
        public virtual List<Role> Roles { get; set; }
        public int? RoleId { get; set; }
        public bool IsInEmployment { get; set; } = true;
        public DateTime? LastDateOfEmployment {get; set; }
        public string Remarks {get; set; }

        public string FullName {get {return FirstName + ", " + FamilyName;} }
    }
}