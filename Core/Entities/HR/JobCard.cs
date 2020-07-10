using System;

namespace Core.Entities.HR
{
    public class JobCard: BaseEntity
    {
        public DateTime JobCardDate { get; set; }

        public int CandidateId { get; set; }
        public int EnquiryItemId { get; set; }
        public bool PPInPossession { get; set; }   
        public bool PPIsValid { get; set; }
        public bool WillingToEmigrate { get; set; }
        public bool WillingToTravelWithinTwoWeeksOfSelection { get; set; }
        public bool RemunerationAcceptable { get; set; }

        public bool ServiceChargesAcceptable { get; set; }
        public bool SuspiciousCandidate { get; set; }
        public bool OkToForwardCVToClient { get; set; }
        public bool OkToConsider { get; set; }
    }
}