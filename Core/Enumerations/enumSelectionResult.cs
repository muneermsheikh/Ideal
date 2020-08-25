using System.Runtime.Serialization;

namespace Core.Enumerations
{
    public enum enumSelectionResult
    {
        [EnumMember(Value = "Referred")]
        Referred=100,
        [EnumMember(Value="Shortlisted")]
        Shortlisted=200,
        [EnumMember(Value="Selected")]
        Selected=300,
        [EnumMember(Value="Deployed")]
        Deployed=400,
        [EnumMember(Value="Rejected-No Reason available")]
        RejectedNoReason=1000,
        [EnumMember(Value="Rejected-Insufficient experience")]
        RejectedInsufficientExp=1100,
        [EnumMember(Value="Rejected-Not qualified")]
        RejectedNotQualified=1200,
        [EnumMember(Value="Rejected-Requirement fulfilled")]
        RejectedRequiremetFulfilled=1300,
        [EnumMember(Value="Rejected-High expectations")]
        RejectedHighExpectations=1400,
        [EnumMember(Value="Rejected - Suspicious profile")]
        RejectedSuspiciousProfile=1500,
        [EnumMember(Value="Rejected-Fake profile")]
        RejectedFakeProfile=1600,
        [EnumMember(Value="Rejected-OverAge")]
        RejectedOverAge=1700

    }
}