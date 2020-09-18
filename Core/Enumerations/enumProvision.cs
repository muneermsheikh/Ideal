using System.Runtime.Serialization;

namespace Core.Enumerations
{
    public enum enumProvision
    {
        [EnumMember(Value = "Provided Free")]
        ProvidedFree,

        [EnumMember(Value = "Not Provided")]
        NotProvided,
        
        [EnumMember(Value = "Allowance Provided")]
        AllowanceProvided
    }
}