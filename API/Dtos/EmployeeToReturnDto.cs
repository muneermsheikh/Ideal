using System.Collections.Generic;
using Core.Entities.HR;
using Core.Entities.Masters;

namespace API.Dtos
{
    public class EmployeeToReturnDto
    {
        public int Id {get; set; }
        public string Gender {get; set; }
        public string FullName { get; set; }
        public string KnownAs { get; set; }
        public string Designation { get; set; }
        public string PassportNo {get; set; }
        public string AadharNo {get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public virtual List<Role> Roles { get; set; }
    }
}