using System;
using Core.Enumerations;

namespace Core.Entities.Admin
{
    public class CVRef: BaseEntity
    {
        public int EnquiryItemId { get; set; }
        public int CandidateId { get; set; }
        public DateTime DateForwarded { get; set; }= DateTime.Now;
        public enumAssessmentResult RefStatus { get; set; } = enumAssessmentResult.Referred_DecisionAwaited;
        public DateTime StatusDate { get; set; }
    }
}