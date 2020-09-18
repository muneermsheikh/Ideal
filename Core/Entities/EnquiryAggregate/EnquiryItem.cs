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

        public EnquiryItem(CategoryItemOrdered itemOrdered, int srNo, int quantity, bool eCNR, DateTime completeBy)
        {
            //EnquiryId = enquiryId;
            SrNo = srNo;
            ItemOrdered = itemOrdered;
            CategoryItemId = itemOrdered.CategoryItemId;
            CategoryName = itemOrdered.CategoryName;
            ECNR = eCNR;
            Quantity = quantity;
            CompleteBy = completeBy;
           }

        public EnquiryItem(int enquiryId, int srNo, int categoryItemId, string categoryName, int quantity, int maxCVsToSend, bool eCNR, bool assessmentReqd, bool evaluationReqd, enumItemReviewStatus status, enumEnquiryStatus enquiryStatus) 
        {
            this.EnquiryId = enquiryId;
                this.SrNo = srNo;
                this.CategoryItemId = categoryItemId;
                this.CategoryName = categoryName;
                this.Quantity = quantity;
                this.MaxCVsToSend = maxCVsToSend;
                this.ECNR = eCNR;
                this.AssessmentReqd = assessmentReqd;
                this.EvaluationReqd = evaluationReqd;
                this.Status = status;
                this.EnquiryStatus = enquiryStatus;
               
        }
                public int EnquiryId { get; set; }
        public virtual CategoryItemOrdered ItemOrdered {get; set; }
        public int SrNo {get; set; }
        public int CategoryItemId { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public int MaxCVsToSend {get; set;}
        public bool ECNR { get; set; } = false;
        public bool AssessmentReqd {get; set; }=false;
        public bool EvaluationReqd {get; set; }=false;
        public int? HRExecutiveId {get; set; }
        public virtual Employee AssessingHRExec {get; set; }
        public int? AssessingSupId { get; set; }
        public int? AssessingHRMId { get; set; }
        public DateTime? CompleteBy { get; set; } = DateTime.Now.AddDays(7);
        public enumItemReviewStatus Status { get; set; } = enumItemReviewStatus.NotReviewed;
        public enumEnquiryStatus EnquiryStatus {get; set;} = enumEnquiryStatus.NotInitiated;

        public virtual Employee AssessingHRM {get; set; }
        public virtual Employee AssessingSup {get; set; }
        public virtual JobDesc JobDesc {get; set; }
        public virtual Remuneration Remuneration {get; set;}
        
        public virtual List<CVRef> CVsReferred {get; set; }

    }


}