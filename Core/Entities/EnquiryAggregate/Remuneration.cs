using System;
using System.ComponentModel.DataAnnotations;
using Core.Enumerations;

namespace Core.Entities.EnquiryAggregate
{
    public class Remuneration: BaseEntity
    {
        public int EnquiryItemId { get; set; }
        public int ContractPeriodInMonths { get; set; }
        [MaxLength(3)]
        public string SalaryCurrency { get; set; }
        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }
        public bool SalaryNegotiable { get; set; }
        public enumProvision Housing { get; set; }
        public int? HousingAllowance { get; set; }
        public enumProvision Food { get; set; }
        public int? FoodAllowance { get; set; }
        public enumProvision Transport { get; set; }
        public int? TransportAllowance { get; set; }
        public int? OtherAllowance { get; set; }
        [Range(0,36)]
        public int LeaveAvailableAfterHowmanyMonths { get; set; }
        [Range(0,90)]
        public int LeaveEntitlementPerYear { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}