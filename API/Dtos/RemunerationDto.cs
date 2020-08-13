namespace API.Dtos
{
    public class RemunerationDto
    {
        public int Id {get; set; }
        public int EnquiryId {get; set; }
        public int EnquiryItemId { get; set; }
        public int ContractPeriodInMonths { get; set; }
        public string SalaryCurrency { get; set; }
        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }
        public bool SalaryNegotiable { get; set; }
        public string Housing { get; set; } 
        public int? HousingAllowance { get; set; }
        public string Food { get; set; }
        public int? FoodAllowance { get; set; } = 0;
        public string Transport { get; set; }
        public int? TransportAllowance { get; set; }
        public int? OtherAllowance { get; set; } 
        public int LeaveAvailableAfterHowmanyMonths { get; set; } 
        public int LeaveEntitlementPerYear { get; set; } 

    }
}