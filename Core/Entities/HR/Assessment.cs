using System;
using System.Collections.Generic;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;

namespace Core.Entities.HR
{
    public class Assessment: BaseEntity
    {
        public Assessment()
        {
        }

        public Assessment(string customerNameAndCity, int enquiryItemId, string categoryNameAndRef, int candidateId, string assessedBy, 
            List<AssessmentItem> assessmentItems)
        {
            CustomerNameAndCity = customerNameAndCity;
            EnquiryItemId = enquiryItemId;
            CategoryNameAndRef = categoryNameAndRef;
            CandidateId = candidateId;
            AssessedBy = assessedBy;
            AssessmentItems = assessmentItems;
        }

        public EnquiryItem Enquiryitem { get; set; }
        public int EnquiryItemId { get; set; }
        public string CustomerNameAndCity {get; set; }
        public string CategoryNameAndRef {get; set; }
        public Candidate Candidate {get; set; }
        public int CandidateId { get; set; }
        public string AssessedBy { get; set; }
        public DateTime AssessedOn { get; set; } = DateTime.Now;
        public List<AssessmentItem> AssessmentItems {get; set; }
        public virtual enumAssessmentResult Result { get; set; }
        public int Grade {get; set;}
        public string GradeString {get; set; }
        public string Remarks {get; set; }
    }
}