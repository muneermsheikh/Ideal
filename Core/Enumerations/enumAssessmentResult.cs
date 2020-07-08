using System.Runtime.Serialization;

namespace Core.Enumerations
{
  public enum enumAssessmentResult
    {
        [EnumMember(Value = "Referred - Decision awaited")]
        Referred_DecisionAwaited,
        [EnumMember(Value = "Shortlisted - First option")]
        Shortlisted_FirstOption,

        [EnumMember(Value = "Shortlisted - second option")]
        Shortlisted_SecondOption,

        [EnumMember(Value = "Shortlisted - third option")]
        Shortlisted_ThirdOption,

        [EnumMember(Value = "Rejected - not qualified")]
        Rejected_NotQualified,

        [EnumMember(Value = "Rejected - no relevant experience")]
        Rejected_NoRelevantExperience,
        
        [EnumMember(Value = "Rejected - high salary expectation")]
        Rejected_HighSalaryExpectation,
        
        [EnumMember(Value = "Rejected - Profile Suspicious")]
        Rejected_ProfileSuspicious,

        [EnumMember(Value = "Rejected - Fake Certificates")]
        Rejected_FakeCertificates
        
    }
}