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

        public Remuneration(int enquiryId, int enquiryItemId, int contractPeriodInMonths, 
            string salaryCurrency, int salaryMin, int salaryMax, bool salaryNegotiable, 
            bool housing, int? housingAllowance, bool food, int? foodAllowance, 
            bool transport, int? transportAllowance, int? otherAllowance, 
            int leaveAvailableAfterHowmanyMonths, int leaveEntitlementPerYear, DateTime updatedOn)
        {
            EnquiryId = enquiryId;
            EnquiryItemId = enquiryItemId;
            ContractPeriodInMonths = contractPeriodInMonths;
            SalaryCurrency = salaryCurrency;
            SalaryMin = salaryMin;
            SalaryMax = salaryMax;
            SalaryNegotiable = salaryNegotiable;
            Housing = housing;
            HousingAllowance = housingAllowance;
            Food = food;
            FoodAllowance = foodAllowance;
            Transport = transport;
            TransportAllowance = transportAllowance;
            OtherAllowance = otherAllowance;
            LeaveAvailableAfterHowmanyMonths = leaveAvailableAfterHowmanyMonths;
            LeaveEntitlementPerYear = leaveEntitlementPerYear;
            UpdatedOn = updatedOn;
        }

        // public int CVRefId {get; set;}
        public int EnquiryId {get; set; }
        public int EnquiryItemId { get; set; }
        public string CategoryName {get; set; }
        // public int CandidateId {get; set;}
        // public DateTime OfferLetterDate {get; set; }
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
        public DateTime UpdatedOn { get; set; } 
    }
}