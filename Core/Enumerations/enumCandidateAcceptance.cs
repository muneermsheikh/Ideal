using System.Runtime.Serialization;

namespace Core.Enumerations
{
  //the order is significant becasue AssessmentSpec compares a parameter with this value.  
  // values at the top have highest weightage.
  public enum enumCandidateAcceptance
    {
        
        [EnumMember(Value="Candidate decision awaited")]
        CandidateDecisionAwaited=0,
        [EnumMember(Value="Accepted")]
        Accepted=100,
        [EnumMember(Value = "Declined due to salary")]
        DeclinedDueToSalary = 1000,

        [EnumMember(Value = "Declined due to perks")]
        DeclinedDueToPerks = 1100,

        [EnumMember(Value = "Declined - employer profile not suitable")]
        DeclinedDueToEmployerProfile = 1200,

        [EnumMember(Value = "Medically Found Unfit")]
        MedicallyUnfitt = 1300,

        [EnumMember(Value = "Declined - Recruitment Fee not acceptable")]
        DeclinedRecruitmentFee=1400
        
    }
}