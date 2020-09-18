using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class AssessmentDto
    {
        public string CustomerNameAndCity {get; set; }
        public string OrderNoAndDate {get; set; }
        public string CategoryNameAndRef {get; set; }
        public int ApplicationNo {get; set;}
        public string CandidateName {get; set; }
        public string AssessedBy { get; set; }
        public DateTime AssessedOn { get; set; } = DateTime.Now;
        public string Result { get; set; }
        public string GradeString {get; set; }
        public string Remarks {get; set; }
        public int TotalCount {get; set; }
        public int AssessmentId {get; set; }
        public int EnquiryItemId { get; set; }
        public List<AssessmentItemDto> Assessments {get; set; }
    }

    public class AssessmentItemDto
    {
        public AssessmentItemDto(int questionNo, bool isMandatory, bool assessed, 
            string assessmentParameter, string question, int maxPoints, int pointsAllotted, 
            int assessmentId)
        {
            QuestionNo = questionNo;
            Assessed = assessed;
            IsMandatory = isMandatory;
            AssessmentParameter = assessmentParameter;
            Question = question;
            MaxPoints = maxPoints;
            PointsAllotted = pointsAllotted;
            AssessmentId = assessmentId;
        }

        public int QuestionNo { get; set; }
        public bool Assessed { get; set; } = false;
        public bool IsMandatory {get; set; }
        public string AssessmentParameter { get; set; }
        public string Question { get; set; }
        public int MaxPoints { get; set; }
        public int PointsAllotted {get; set;}
        public string Remarks { get; set; }
        public int AssessmentId { get; set; }       
    }
}