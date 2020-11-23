using System;
using Core.Enumerations;

namespace Core.Entities.EnquiryAggregate
{
    public class BasketItem
    {
        public BasketItem()
        {
        }

        public BasketItem(int categoryId, string categorytName, string eCNR, int quantity, 
            int expDesiredInYrsMin, int expDesiredInYrsMax, string jobDescInBrief, 
            string jobDescAttachment, string salaryCurrency, string salaryNegotiable, 
            int salaryRangeMin, int salaryRangeMax, int contractPeriodInMonths, 
            int leaveAfterMonths, int leavePerYear, string food, int foodAllowance,
            string housing, int housingAllowance, string transport, 
            int transportAllowance, int otherAllowance, DateTime completeBy)
        {
            // Id = id;
            CategoryId = categoryId;
            CategorytName = categorytName;
            Ecnr = eCNR;
            Quantity = quantity;
            ExpDesiredInYrsMin = expDesiredInYrsMin;
            ExpDesiredInYrsMax = expDesiredInYrsMax;
            JobDescInBrief = jobDescInBrief;
            JobDescAttachment = jobDescAttachment;
            SalaryCurrency = SalaryCurrency;
            SalaryRangeMin = salaryRangeMin;
            SalaryRangeMax = salaryRangeMax;
            ContractPeriodInMonths = contractPeriodInMonths;
            Food = food;
            FoodAllowance = foodAllowance;
            Housing = housing;
            HousingAllowance = housingAllowance;
            Transport = transport;
            TransportAllowance = transportAllowance;
            OtherAllowance = otherAllowance;
            CompleteBy = completeBy;
            LeaveAfterMonths= leaveAfterMonths;
            LeavePerYear = leavePerYear;
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategorytName { get; set; }
        public string Ecnr { get; set; }
        public int Quantity { get; set; }
        public int ExpDesiredInYrsMin { get; set; }
        public int ExpDesiredInYrsMax { get; set; }
        public string JobDescInBrief { get; set; }
        public string JobDescAttachment { get; set; }
        public string SalaryCurrency {get; set; }

        public string SalaryNegotiable {get; set; }
        public int SalaryRangeMin { get; set; }
        public int SalaryRangeMax { get; set; }
        public int ContractPeriodInMonths { get; set; }
        public string Food { get; set; }
        public int FoodAllowance {get; set; }
        public string Housing { get; set; }
        public int HousingAllowance {get; set; }
        public string Transport { get; set; }
        public int TransportAllowance {get; set; }
        public int OtherAllowance {get; set; }
        public DateTime CompleteBy { get; set; }
        public int LeaveAfterMonths {get; set; }
        public int LeavePerYear {get; set; }
    }
}