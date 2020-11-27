using System;
using System.Collections.Generic;
using Core.Entities.EnquiryAggregate;

namespace API.Dtos
{
    public class EnquiryItemForClient
    {
        public int Id {get; set; }
        public int CategoryItemId { get; set; }
        public string CategoryName { get; set; }
        public string ECNR { get; set; }
        public string AssessmentRequired {get; set;}
        public string EvaluationRequired {get; set; }
        public int Quantity { get; set; }
        public int MaxCVsToForward {get; set; }
        public DateTime CompleteBy { get; set; }
        public string HRExecutiveName {get; set;}
        
        public string AssessingSup {get; set;}
        public string AssessingHRM {get; set;}
        public string ReviewStatus {get; set;}
        public string Status {get; set;}
    }
}