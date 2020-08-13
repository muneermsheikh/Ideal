using System;
using System.Collections.Generic;
using Core.Entities.EnquiryAggregate;

namespace Core.Entities.Admin
{
    public class DLForwarded
    {
        public DLForwarded()
        {
        }

        public int Id {get; set; }
        public int AssignedTo  { get; set; } 
        //public string AssignedToName {get; set; }
        public DateTime AssignedOn { get; set; } 
        public IReadOnlyList<Enquiry> Enquiries { get; set; }
    }
}