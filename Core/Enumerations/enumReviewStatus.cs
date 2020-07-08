using System.Runtime.Serialization;

namespace Core.Enumerations
{
    public enum enumReviewStatus
    {
        [EnumMember(Value = "Not Reviewed")]
        NotReviewed,
        
        [EnumMember(Value = "Accepted")]
        Accepted,

        [EnumMember(Value = "Accepted - with some exceptions")]
        Accepted_WithExceptions,

        [EnumMember(Value = "Declined")]
        Declined
    }
}