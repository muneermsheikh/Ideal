using Core.Entities.EnquiryAggregate;

namespace Core.Entities.HR
{
    public class AssessmentQ: BaseEntity
    {
        public AssessmentQ()
        {
        }

        public AssessmentQ(int assessmentId, int enquiryItemId, int enquiryId, int questionNo, string question, int maxPoints  )
        {
            AssessmentId = assessmentId;
            EnquiryItemId = enquiryItemId;
            EnquiryId = enquiryId;
            QuestionNo = questionNo;
            Question = question;
            MaxPoints = maxPoints;
        }

        public int AssessmentId { get; set; }
        public int EnquiryItemId { get; set; }
        public int EnquiryId { get; set; }
        public EnquiryItem EnquiryItem { get; set; }
        public int QuestionNo { get; set; }
        public bool IsMandatory {get; set; }
        public string DomainSubject { get; set; }
        public string AssessmentParameter { get; set; }
        public string Question { get; set; }
        public int MaxPoints { get; set; }
        public string Remarks { get; set; }
    }
}