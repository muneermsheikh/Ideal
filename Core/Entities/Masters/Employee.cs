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

        public Employee(Person person, EmployeeAddress employeeAddress, 
            string designation, DateTimeOffset dOJ)
        {
            Person = person;
            Designation = designation;
            DOJ = dOJ;
            EmployeeAddress = employeeAddress;
        }

        public Person Person { get; set; }
        public string Designation { get; set; }
        public DateTimeOffset DOJ { get; set; }
        public virtual EmployeeAddress EmployeeAddress { get; set; }
        public virtual List<Role> Roles { get; set; }
        public int? RoleId { get; set; }
        public bool IsInEmployment { get; set; } = true;
        public DateTimeOffset LastDateOfEmployment {get; set; }
        public string Remarks {get; set; }
    }
}