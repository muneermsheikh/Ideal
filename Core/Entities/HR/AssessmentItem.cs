using System.ComponentModel.DataAnnotations;

namespace Core.Entities.HR
{
    public class AssessmentItem: BaseEntity
    {
        public AssessmentItem()
        {
        }

        public AssessmentItem(int questionNo, bool isMandatory, string domainSubject, 
            string assessmentParameter, string question, int maxPoints)
        {
            IsMandatory = isMandatory;
            QuestionNo = questionNo;
            DomainSubject = domainSubject;
            AssessmentParameter = assessmentParameter;
            Question = question;
            MaxPoints = maxPoints;
        }

        public int AssessmentId { get; set; }
        public bool Assessed { get; set; } = false;
        public bool IsMandatory {get; set; }
        public int QuestionNo { get; set; }
        public string DomainSubject { get; set; }
        public string AssessmentParameter { get; set; }
        public string Question { get; set; }
        public string Remarks { get; set; }
        public int MaxPoints { get; set; }
        public int PointsAllotted {get; set;}

    }
}