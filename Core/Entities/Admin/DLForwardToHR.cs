using System;
using System.Collections.Generic;
using Core.Entities.EnquiryAggregate;

// this is triggered by Enquiry.EnquiryStatus value updated to Approved.
namespace Core.Entities.Admin
{
    public class DLForwardToHR: BaseEntity
    {
        public DLForwardToHR()
        {
        }

        public DLForwardToHR(int assignedTo, DateTime assignedOn, int enquiryId)
        {
            AssignedTo = assignedTo;
            AssignedOn = assignedOn;
            EnquiryId = enquiryId;
        }

        public int AssignedTo  { get; set; }        // task assigned To
        public DateTime AssignedOn { get; set; } = DateTime.Now;
        public int EnquiryId { get; set; }
        public Enquiry EnquiryWithItems {get; set; }
        public ToDo ToDo {get; set; }   // these enquriyitems are assigned to the   HR Supervisor as task
        public string Remarks { get; set; }
    }
}