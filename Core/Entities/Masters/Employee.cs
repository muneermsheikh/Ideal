using System;
using System.Collections.Generic;
using Core.Entities.HR;

namespace Core.Entities.Masters
{
    public class Employee: BaseEntity
    {
        public Person Person { get; set; }
        public string Designation { get; set; }
        public DateTime DOJ { get; set; }
        public virtual EmployeeAddress EmployeeAddress { get; set; }
        public virtual List<Role> Roles { get; set; }
        public int? RoleId { get; set; }
        public bool IsInEmployment { get; set; } = true;
    }
}