using System;
using System.ComponentModel.DataAnnotations;
using Core.Enumerations;

namespace Core.Entities.EnquiryAggregate
{
    public class Remuneration: BaseEntity
    {
        public Remuneration()
        {
        }

        public Remuneration(int enquiryItemId, int enquiryId)
        {
            EnquiryId = enquiryId;
            EnquiryItemId = enquiryItemId;
        }

        public int EnquiryId {get; set; }
        public int EnquiryItemId { get; set; }
        public int ContractPeriodInMonths { get; set; }
        [MaxLength(3)]
        public string SalaryCurrency { get; set; }
        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }
        public bool SalaryNegotiable { get; set; }=false;
        public enumProvision Housing { get; set; } = enumProvision.Provided_Free;
        public int? HousingAllowance { get; set; } = 0;
        public enumProvision Food { get; set; } = enumProvision.NotProvided;
        public int? FoodAllowance { get; set; } = 0;
        public enumProvision Transport { get; set; } = enumProvision.Provided_Free;
        public int? TransportAllowance { get; set; } = 0;
        public int? OtherAllowance { get; set; } = 0;
        [Range(0,36)]
        public int LeaveAvailableAfterHowmanyMonths { get; set; } = 24;
        [Range(0,90)]
        public int LeaveEntitlementPerYear { get; set; } = 21;
        public DateTimeOffset UpdatedOn { get; set; } = DateTimeOffset.Now;
    }
}