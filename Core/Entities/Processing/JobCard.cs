using System;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Masters;

namespace Core.Entities.Processing
{
    public class JobCardHR: BaseEntity
    {
        public int CandidateId { get; set; }
        public EnquiryItem EnquiryItem { get; set; }
        public int EnquiryItemId { get; set; }
        public Employee HRExecutive { get; set; }
        public int HRExecutiveId { get; set; }
        public DateTime JobCardDate { get; set; } = DateTime.Now;
        public bool PPInPossession { get; set; }
        public bool PPIsValid { get; set; }
        public string PPException { get; set; }
        public bool WillingToEmigrate { get; set; }
        public bool RemunerationAcceptable { get; set; }
        public string RemunerationException { get; set; }
        public bool ServiceChargesAcceptable { get; set; }
        public string ServiceChargesException { get; set; }
        public bool WillingToTravelWithin15DaysOfSelection { get; set; }
        public bool OkToForwardCVToClient { get; set; }
        public bool SuspiciousCandidate { get; set; }
        public string Remarks { get; set; }
        public bool OkToConsider { get; set; }
    }
}