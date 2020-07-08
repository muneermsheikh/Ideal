using System.Runtime.Serialization;

namespace Core.Enumerations
{
   public enum enumCandidateStatus
    {
        [EnumMember(Value = "Available")]
        Available,

        [EnumMember(Value = "Referred")]
        Referred,

        [EnumMember(Value = "Shortlisted - Second option")]
        Shortlisted_SecondOption,

        [EnumMember(Value = "Shortlisted - Third option")]
        Shortlisted_ThirdOption,

        [EnumMember(Value = "Shortlisted - First option")]
        Shortlisted_FirstOption,

        [EnumMember(Value = "Not Available - Medically Unfit")]
        NotAvailable_MedicallyUnfit,

        [EnumMember(Value = "Not Available - Blacklisted")]
        NotAvailable_Blacklisted
    }
}