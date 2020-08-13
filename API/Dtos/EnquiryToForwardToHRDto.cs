using System;
using System.Collections.Generic;
using Core.Entities.Admin;

namespace API.Dtos
{
    public class EnquiryToForwardToHRDto
    {
        public DateTime dtForwarded {get; set;}
        public int EmployeeId {get; set; }        
        public IReadOnlyList<IdInt> enquiryIds {get; set; }
    }
}