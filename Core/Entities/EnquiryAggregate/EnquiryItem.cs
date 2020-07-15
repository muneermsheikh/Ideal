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
            enumProvision food, enumProvision housing, enumProvision transport, DateTimeOffset completeBy)
        {
            //EnquiryId = enquiryId;
            ItemOrdered = itemOrdered;
            CategoryItemId = itemOrdered.CategoryItemId;
            CategoryName = itemOrdered.CategoryName;
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
            CompleteBy = completeBy;
        }

        public int EnquiryId { get; set; }
        public CategoryItemOrdered ItemOrdered {get; set; }
        public int CategoryItemId { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public bool ECNR { get; set; } = false;
        public int ExpDesiredInYrsMin { get; set; }
        public int ExpDesiredInYrsMax { get; set; }
        public string JobDescInBrief { get; set; }
        public string JobDescAttachment { get; set; }
        public int SalaryRangeMin { get; set; }
        public int SalaryRangeMax { get; set; }
        public int ContractPeriodInMonths { get; set; } = 24;
        public enumProvision Food { get; set; } = enumProvision.NotProvided;
        public enumProvision Housing { get; set; } = enumProvision.NotProvided;
        public enumProvision Transport { get; set; } = enumProvision.NotProvided;
        public DateTimeOffset CompleteBy { get; set; } = DateTimeOffset.Now.AddDays(7);
        public enumItemReviewStatus Status { get; set; } = enumItemReviewStatus.NotReviewed;
        public JobDesc JobDesc {get; set; }
        public int JobDescId { get; set; }
        public Remuneration Remuneration {get; set;}
        public int RemunerationId { get; set; }
    }


}