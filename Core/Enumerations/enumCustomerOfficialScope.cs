using System.Runtime.Serialization;

namespace Core.Enumerations
{
    public enum enumCustomerOfficialScope
    {
         [EnumMember(Value = "HR")]
         HR = 1000,
         [EnumMember(Value = "Accounts")]
         Accounts = 3000,
         [EnumMember(Value = "Logistics")]
         Logistics = 2000,
         [EnumMember(Value="Personnel")]
         Personnel = 4000
    }
}