using System;
using Core.Entities.Masters;
using Core.Enumerations;
using Core.Interfaces;

namespace Core.Entities.EnquiryAggregate
{
    public class EnquiryItem : BaseEntity
    {

        public EnquiryItem()
        {
        }

        public EnquiryItem(CategoryItemOrdered itemOrdered, int quantity, bool eCNR, 
            int expDesiredInYrsMin, int expDesiredInYrsMax, string jobDescInBrief,
            string jobDescAttachment, int salaryRangeMin, int salaryRangeMax, int contractPeriodInMonths,
            enumProvision food, enumProvision housing, enumProvision transport, DateTime dateRequiredBy)
        {
            ItemOrdered = itemOrdered;
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
        }

        public CategoryItemOrdered ItemOrdered {get; set; }
        public int Quantity { get; set; }
        public bool ECNR { get; set; }
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