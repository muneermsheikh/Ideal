using System.Runtime.Serialization;

namespace Core.Enumerations
{
    public enum enumItemReviewStatus
    {
        [EnumMember(Value = "Not Reviewed")]
        NotReviewed,

        [EnumMember(Value = "Accepted")]
        Accepted,

        [EnumMember(Value = "Rejected - technically not feasible")]
        Rejected_TechNotFeasible,
        
        [EnumMember(Value = "Rejected - Salary Offered not feasible")]
        Rejected_SalaryOfferedNotFeasible,
        
        [EnumMember(Value = "Rejected - Low standing of customer")]
        Rejected_CustomerLowStanding,

        [EnumMember(Value = "Rejected - Visas not available")]
        Rejected_VisasNotAvailable,

        [EnumMember(Value = "Rejected - Requirement Suspect")]
        Rejected_RequirementSuspect
    }
}