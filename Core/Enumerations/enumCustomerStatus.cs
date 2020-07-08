using System.Runtime.Serialization;

namespace Core.Enumerations
{
    public enum enumCustomerStatus
    {
        [EnumMember(Value = "Active")]
        Active,

        [EnumMember(Value = "In business, but not active with us")]
        InBusinessButNotActiveWithUs,

        [EnumMember(Value = "Closed down")]
        Closed

    }
}