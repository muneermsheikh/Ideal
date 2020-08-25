using System.Runtime.Serialization;

namespace Core.Enumerations
{
    public enum enumEnquiryStatus
    {
        [EnumMember(Value = "Review Pending")]
        ReviewPending,

        [EnumMember(Value = "Accepted")]
        ReviewedAndAccepted,

        [EnumMember(Value = "Rejected")]
        ReviewedAndRejected
    }
}