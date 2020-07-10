using System;
using Core.Enumerations;

namespace Core.Entities.EnquiryAggregate
{
    public class BasketItem
    {
        public BasketItem()
        {
        }

        public BasketItem(int categoryId, string categorytName, bool eCNR, int quantity, 
            int expDesiredInYrsMin, int expDesiredInYrsMax, string jobDescInBrief, 
            string jobDescAttachment, int salaryRangeMin, int salaryRangeMax, 
            int contractPeriodInMonths, enumProvision food, enumProvision housing, 
            enumProvision transport, DateTime dateRequiredBy)
        {
            // Id = id;
            CategoryId = categoryId;
            CategorytName = categorytName;
            ECNR = eCNR;
            Quantity = quantity;
            ExpDesiredInYrsMin = expDesiredInYrsMin;
            ExpDesiredInYrsMax = expDesiredInYrsMax;
            JobDescInBrief = jobDescInBrief;
            JobDescAttachment = jobDescAttachment;
            SalaryRangeMin = salaryRangeMin;
            SalaryRangeMax = salaryRangeMax;
            ContractPeriodInMonths = contractPeriodInMonths;
            Food = food;
            Housing = housing;
            Transport = transport;
            DateRequiredBy = dateRequiredBy;
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategorytName { get; set; }
        public bool ECNR { get; set; }
        public int Quantity { get; set; }
        public int ExpDesiredInYrsMin { get; set; }
        public int ExpDesiredInYrsMax { get; set; }
        public string JobDescInBrief { get; set; }
        public string JobDescAttachment { get; set; }
        public int SalaryRangeMin { get; set; }
        public int SalaryRangeMax { get; set; }
        public int ContractPeriodInMonths { get; set; }
        public enumProvision Food { get; set; }
        public enumProvision Housing { get; set; }
        public enumProvision Transport { get; set; }
        public DateTime DateRequiredBy { get; set; }
    }
}