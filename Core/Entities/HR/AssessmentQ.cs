using Core.Entities.EnquiryAggregate;

namespace Core.Entities.HR
{
    public class AssessmentQ: BaseEntity
    {
        public int AssessmentId { get; set; }
        public int EnquiryItemId { get; set; }
        public EnquiryItem EnquiryItem { get; set; }
        public int QuestionNo { get; set; }
        public string Question { get; set; }
        public bool Assessed { get; set; }
        public int MaxPoints { get; set; }
        public int PointsGiven { get; set; }
        public string Remarks { get; set; }
    }
}