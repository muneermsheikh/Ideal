namespace Core.Entities.Masters
{
    public class AssessmentQBank: BaseEntity
    {
        public AssessmentQBank()
        {
        }

        public int SrNo {get; set; }
        public int DomainSubId { get; set; }
        public bool IsStandardQuestion { get; set; }
        public string AssessmentParameter { get; set; }
        public string Question { get; set; }
        public int MaxPoints {get; set; }
        public DomainSub DomainSubj { get; set; }
    }
}