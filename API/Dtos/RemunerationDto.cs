using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class RemunerationDto
    {
        public int EnquiryId {get; set; }
        public int EnquiryItemId { get; set; }
        public string CategoryName {get; set; }
        public int ContractPeriodInMonths { get; set; }
        [MaxLength(3)]
        public string SalaryCurrency { get; set; }
        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }
        public bool SalaryNegotiable { get; set;}
        public bool Housing { get; set; }
        public int? HousingAllowance { get; set; }
        public bool Food { get; set; } 
        public int? FoodAllowance { get; set; } = 0;
        public bool Transport { get; set; } 
        public int? TransportAllowance { get; set; } = 0;
        public int? OtherAllowance { get; set; } = 0;
        [Range(0,36)]
        public int LeaveAvailableAfterHowmanyMonths { get; set;}
        [Range(0,90)]
        public int LeaveEntitlementPerYear { get; set; } 

    }
}