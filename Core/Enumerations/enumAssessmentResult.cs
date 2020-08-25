using System.Runtime.Serialization;

namespace Core.Enumerations
{
  //the order is significant becasue AssessmentSpec compares a parameter with this value.  
  // values at the top have highest weightage.
  public enum enumAssessmentResult
    {
        
        [EnumMember(Value = "Shortlisted - First option")]
        Shortlisted_FirstOption = 100,

        [EnumMember(Value = "Shortlisted - second option")]
        Shortlisted_SecondOption = 200,

        [EnumMember(Value = "Shortlisted - third option")]
        Shortlisted_ThirdOption = 300,

        [EnumMember(Value = "Rejected - not qualified")]
        Rejected_NotQualified = 1000,

        [EnumMember(Value = "Rejected - no relevant experience")]
        Rejected_NoRelevantExperience = 1100,
        
        [EnumMember(Value = "Rejected - high salary expectation")]
        Rejected_HighSalaryExpectation=1200,
        
        [EnumMember(Value = "Rejected - Profile Suspicious")]
        Rejected_ProfileSuspicious=1300,

        [EnumMember(Value = "Rejected - Fake Certificates")]
        Rejected_FakeCertificates=1400,
        
        [EnumMember(Value = "Referred")]
        Referred=10000
        
    }
}