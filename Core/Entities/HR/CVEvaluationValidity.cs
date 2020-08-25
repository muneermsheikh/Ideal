namespace Core.Entities.HR
{
    public class CVEvaluationValidity
    {
        public bool Validated {get; set;}
        public Assessment Assessment {get; set;}
        public int supId {get; set; }
        public int HRMId {get; set; }
        public string NextPhase {get; set; }
        public string ErrorMessage {get; set; }
    }
}