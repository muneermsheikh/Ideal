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

        public EnquiryItem(int enquiryId, int srNo, int categoryItemId, string categoryName, 
            int quantity, int maxCVsToSend, string eCNR, string assessmentReqd, 
            string evaluationReqd, string reviewStatus, string enquiryStatus) 
        {
            this.EnquiryId = enquiryId;
                this.SrNo = srNo;
                this.CategoryItemId = categoryItemId;
                this.CategoryName = categoryName;
                this.Quantity = quantity;
                this.MaxCVsToSend = maxCVsToSend;
                this.Ecnr = eCNR;
                this.AssessmentReqd = assessmentReqd;
                this.EvaluationReqd = evaluationReqd;
                this.ReviewStatus = reviewStatus;
                this.EnquiryStatus = enquiryStatus;
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