using Core.Entities.Masters;

namespace Core.Entities.HR
{
    public class EnquiryItemAssessmentQ: BaseEntity
    {
        public EnquiryItemAssessmentQ()
        {
        }

        public int EnquiryItemId { get; set; }
        public int SrNo { get; set; }
        public int DomainSubjectId { get; set; }
        public DomainSub DomainSub { get; set; }
        public string AssessmentParameter { get; set; }
        public string Question { get; set; }
        public bool Mandatory { get; set; }=true;
        public int MaxPoints { get; set; }
        public string Remarks { get; set; }
    }
}