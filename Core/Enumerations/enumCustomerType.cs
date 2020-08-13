using System.Runtime.Serialization;

namespace Core.Enumerations
{
    public enum enumCustomerType
    {
        [EnumMember(Value = "Customer")]
        Customer,
        [EnumMember(Value = "Associate")]
        Associate,
        [EnumMember(Value = "Supplier")]
        Supplier,
        [EnumMember(Value = "Candidate")]
        Candidate
    }
}