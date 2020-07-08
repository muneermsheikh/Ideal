using System.Runtime.Serialization;

namespace Core.Enumerations
{
    public enum enumTaskStatus
    {
        [EnumMember(Value = "Not Started")]
        NotStarted,

        [EnumMember(Value = "Started")]
        Started,

        [EnumMember(Value = "Completed")]
        Completed,

        [EnumMember(Value = "Canceled")]
        Canceled
    }
}