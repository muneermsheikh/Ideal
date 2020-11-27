using System;
using System.Collections.Generic;
using Core.Entities.EnquiryAggregate;

namespace API.Dtos
{
    public class EnqToAddDto
    {
        public int CustomerId {get; set; }
        public int EnquiryNo { get; set; }
        public string BasketId {get; set;}
        public DateTime EnquiryDate { get; set; } = DateTime.Now;
        
        public string ReadyToReview {get; set; } = "f";         // this is set to true when all enquiryitems are reviewed
        public string EnquiryStatus { get; set; }="NotActive";
        
        public int ProjectManagerId {get; set; }
        public int? AccountExecutiveId {get; set; }
        public int? HRExecutiveId {get; set; }
        public int? LogisticsExecutiveId {get; set; }
        public string EnquiryRef { get; set; }
        public DateTime? CompleteBy { get; set; }

        public int? ReviewedById {get; set; }
        public string ReviewStatus {get; set;}
        public DateTime? ReviewedOn { get; set; }
        public string Remarks { get; set; }

        public List<EnquiryItem> EnquiryItems { get; set; }
    }

    public class EnqItemToAddDto
    {
        public int Id {get; set;}
        public string SrNo {get; set; }
        public int CategoryItemId { get; set; }
        public string CategoryName { get; set; }
        public string Quantity { get; set; }
        public string MaxCVsToSend {get; set;}
        public string Ecnr { get; set; } = "f";
        public string AssessmentReqd {get; set; }="f";
        public string EvaluationReqd {get; set; }="f";
        
        public int? HRExecutiveId {get; set; }
        public int? AssessingSupId { get; set; }
        public int? AssessingHRMId { get; set; }
        public DateTime? CompleteBy { get; set; } 
        public string ReviewStatus { get; set; } 
        public string EnquiryStatus {get; set;}
        public string Charges {get; set;} 
    }

}