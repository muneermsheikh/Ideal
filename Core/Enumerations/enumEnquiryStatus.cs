using System.Runtime.Serialization;

namespace Core.Enumerations
{
    public enum enumEnquiryStatus
    {
        [EnumMember(Value = "Not initiated")]
        NotInitiated,

        [EnumMember(Value = "In Process")]
        InProcess,

        [EnumMember(Value = "Concluded")]
        Concluded
    }
}