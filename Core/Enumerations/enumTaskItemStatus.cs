using System.Runtime.Serialization;

namespace Core.Enumerations
{
    public enum enumTaskItemStatus
    {
        [EnumMember(Value = "In Process")]
        InProcess,
        [EnumMember(Value = "Completed")]
        Completed,
        [EnumMember(Value = "Canceled")]
        Canceled
    }
}