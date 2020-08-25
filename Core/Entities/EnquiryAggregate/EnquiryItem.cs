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

        public EnquiryItem(CategoryItemOrdered itemOrdered, int srNo, int quantity, bool eCNR, DateTime completeBy)
            /*int expDesiredInYrsMin, int expDesiredInYrsMax, string jobDescInBrief,
            string jobDescAttachment, int salaryRangeMin, int salaryRangeMax, int contractPeriodInMonths,
            enumProvision food, enumProvision housing, enumProvision transport, DateTime completeBy) */
        {
            //EnquiryId = enquiryId;
            SrNo = srNo;
            ItemOrdered = itemOrdered;
            CategoryItemId = itemOrdered.CategoryItemId;
            CategoryName = itemOrdered.CategoryName;
            ECNR = eCNR;
            Quantity = quantity;
        /*    
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
        */
            CompleteBy = completeBy;
           }

        public int EnquiryId { get; set; }
        public CategoryItemOrdered ItemOrdered {get; set; }
        public int SrNo {get; set; }
        public int CategoryItemId { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public bool ECNR { get; set; } = false;
        public bool AssessmentReqd {get; set; }=false;
        public bool EvaluationReqd {get; set; }=false;
        public int? HRExecutiveId {get; set; }
        public virtual Employee AssessingHRExec {get; set; }
        public int? AssessingSupId { get; set; }
        public virtual Employee AssessingSup {get; set; }
        public int? AssessingHRMId { get; set; }
        public virtual Employee AssessingHRM {get; set; }
        
        public virtual JobDesc JobDesc {get; set; }
        //public int? JobDescId { get; set; }
    /*
        public int ExpDesiredInYrsMin { get; set; }=0;
        public int ExpDesiredInYrsMax { get; set; }=0;
        public string JobDescInBrief { get; set; }
        public string JobDescAttachment { get; set; }
    */

        public virtual Remuneration Remuneration {get; set;}
        //public int? RemunerationId { get; set; }

    /*
        public int SalaryRangeMin { get; set; }=0;
        public int SalaryRangeMax { get; set; }=0;
        public int ContractPeriodInMonths { get; set; } = 24;
        
        public enumProvision? Food { get; set; } = enumProvision.NotProvided;
        public enumProvision? Housing { get; set; } = enumProvision.NotProvided;
        public enumProvision? Transport { get; set; } = enumProvision.NotProvided;
    */
        public DateTime? CompleteBy { get; set; } = DateTime.Now.AddDays(7);
        public enumItemReviewStatus Status { get; set; } = enumItemReviewStatus.NotReviewed;

    }


}