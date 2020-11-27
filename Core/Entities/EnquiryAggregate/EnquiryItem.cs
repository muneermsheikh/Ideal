using System;
using System.Collections.Generic;
using Core.Entities.Admin;
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

        public EnquiryItem(CategoryItemOrdered itemOrdered, int srNo, int quantity, string eCNR, DateTime completeBy)
        {
            //EnquiryId = enquiryId;
            SrNo = srNo;
            ItemOrdered = itemOrdered;
            CategoryItemId = itemOrdered.CategoryItemId;
            CategoryName = itemOrdered.CategoryName;
            Ecnr = eCNR;
            Quantity = quantity;
            CompleteBy = completeBy;
           }

        public EnquiryItem(int srNo,  string categoryName, 
            int quantity, int maxCVsToSend, string ecnr, string assessmentReqd, 
            string evaluationReqd, string reviewStatus, string enquiryStatus) 
        {
                this.SrNo = srNo;
                this.CategoryName = categoryName;
                this.Quantity = quantity;
                this.MaxCVsToSend = maxCVsToSend;
                this.Ecnr = ecnr;
                this.AssessmentReqd = assessmentReqd;
                this.EvaluationReqd = evaluationReqd;
                this.ReviewStatus = reviewStatus;
                this.EnquiryStatus = enquiryStatus;
        }

        public EnquiryItem(int enquiryId, CategoryItemOrdered itemOrdered, int srNo, int categoryItemId, string categoryName, int quantity, int maxCVsToSend, string ecnr, string assessmentReqd, string evaluationReqd, int? hRExecutiveId, Employee assessingHRExec, int? assessingSupId, int? assessingHRMId, DateTime? completeBy, string reviewStatus, string enquiryStatus, string charges, Employee assessingHRM, Employee assessingSup, JobDesc jobDesc, Remuneration remuneration, List<CVRef> cVsReferred, List<ToDo> tasksAssigned)
        {
            EnquiryId = enquiryId;
            ItemOrdered = itemOrdered;
            SrNo = srNo;
            CategoryItemId = categoryItemId;
            CategoryName = categoryName;
            Quantity = quantity;
            MaxCVsToSend = maxCVsToSend;
            Ecnr = ecnr;
            AssessmentReqd = assessmentReqd;
            EvaluationReqd = evaluationReqd;
            HRExecutiveId = hRExecutiveId;
            AssessingHRExec = assessingHRExec;
            AssessingSupId = assessingSupId;
            AssessingHRMId = assessingHRMId;
            CompleteBy = completeBy;
            ReviewStatus = reviewStatus;
            EnquiryStatus = enquiryStatus;
            Charges = charges;
            AssessingHRM = assessingHRM;
            AssessingSup = assessingSup;
            JobDesc = jobDesc;
            Remuneration = remuneration;
            CVsReferred = cVsReferred;
            TasksAssigned = tasksAssigned;
        }

        public int EnquiryId { get; set; }
        public virtual CategoryItemOrdered ItemOrdered {get; set; }
        public int SrNo {get; set; }
        public int CategoryItemId { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public int MaxCVsToSend {get; set;}
        public string Ecnr { get; set; } = "f";
        public string AssessmentReqd {get; set; }="f";
        public string EvaluationReqd {get; set; }="f";
        
        public int? HRExecutiveId {get; set; }
        public virtual Employee AssessingHRExec {get; set; }
        public int? AssessingSupId { get; set; }
        public int? AssessingHRMId { get; set; }
        public DateTime? CompleteBy { get; set; } = DateTime.Now.AddDays(7);
        public string ReviewStatus { get; set; } = "NotReviewed";
        public string EnquiryStatus {get; set;}
        public string Charges {get; set;} 

        public virtual Employee AssessingHRM {get; set; }
        public virtual Employee AssessingSup {get; set; }
        public virtual JobDesc JobDesc {get; set; }
        public virtual Remuneration Remuneration {get; set;}
        
        public virtual List<CVRef> CVsReferred {get; set; }
        public virtual List<ToDo> TasksAssigned {get; set;}

    }


}