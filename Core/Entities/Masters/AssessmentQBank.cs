namespace Core.Entities.Masters
{
    public class AssessmentQBank: BaseEntity
    {
        public AssessmentQBank()
        {
        }

        public AssessmentQBank(int srNo, int categoryId, bool isStandardQuestion, bool isMandatory,
            string assessmentParameter, string question, DomainSub domainSubj, int maxPoints)
        {
            SrNo = srNo;
            CategoryId = categoryId;
            IsStandardQuestion = isStandardQuestion;
            IsMandatory=isMandatory;
            AssessmentParameter = assessmentParameter;
            Question = question;
            MaxPoints = maxPoints;
        }

        public int SrNo {get; set; }
        public int CategoryId {get; set;}
        public bool IsMandatory {get; set; }
        public bool IsStandardQuestion { get; set; }
        public string AssessmentParameter { get; set; }
        public string Question { get; set; }
        public int MaxPoints {get; set; }

    }
}