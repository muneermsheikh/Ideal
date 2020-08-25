namespace API.Dtos
{
    public class AssessmentItemToAddDto
    {
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