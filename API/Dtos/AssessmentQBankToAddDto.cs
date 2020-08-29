using Core.Entities.Masters;

namespace API.Dtos
{
    public class AssessmentQBankToAddDto
    {
        public int SrNo {get; set; }
        public string CategoryRef { get; set; }
        public bool IsStandardQuestion { get; set; }
        public string AssessmentParameter { get; set; }
        public string Question { get; set; }
        public int MaxPoint {get; set; }
 
    }
}