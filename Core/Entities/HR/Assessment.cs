using System;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;

namespace Core.Entities.HR
{
    public class Assessment: BaseEntity
    {
        public EnquiryItem Enquiryitem { get; set; }
        public int EnquiryItemId { get; set; }
        public Candidate Candidate {get; set; }
        public int CandidateId { get; set; }
        public virtual enumAssessmentResult Result { get; set; }
        public int? AssessmentResultId { get; set; }
        public string AssessedBy { get; set; }
        public DateTime AssessedOn { get; set; } = DateTime.Now;
    }
}