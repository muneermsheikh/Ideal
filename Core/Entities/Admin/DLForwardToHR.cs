using System;
using System.Collections.Generic;
using Core.Entities.EnquiryAggregate;

// this is triggered by Enquiry.EnquiryStatus value updated to Approved.
namespace Core.Entities.Admin
{
    public class DLForwardToHR: BaseEntity
    {
        public int AssignedTo  { get; set; }        // task assigned To
        public DateTimeOffset AssignedOn { get; set; } = DateTimeOffset.Now;
        public List<EnquiryItem> EnquiryItems { get; set; }
        public ToDo ToDo {get; set; }   // these enquriyitems are assigned to the   HR Supervisor as task
        public string Remarks { get; set; }
    }
}