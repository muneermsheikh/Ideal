using System.Runtime.Serialization;

namespace Core.Enumerations
{
   public enum enumCustomerGrade
    {
        [EnumMember(Value = "Loyal, but not much business potential")]
        Loyal_ButNoBusinessPotential,

        [EnumMember(Value = "Loyal, High business potential")]
        Loyal_HighBusinessPotential,

        [EnumMember(Value = "Loyal, business turnover 50-100 per year")]
        Loyal_Turnover50to100PerYear,

        [EnumMember(Value = "Loyal, business turnover 101-150 per year")]
        Loyal_Turnover100to150PerYear,

        [EnumMember(Value = "Loyal, business turnover 150+ per year")]
        Loyal_TurnoverMoreThan150PerYear,

        [EnumMember(Value = "Business turnover 150+ year, but not Loyal")]
        BusinessPotentialMoreThan150PerYearButNotLoyal

    }
}