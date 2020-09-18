using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class ProcessDto
    {
        public int CVRefId { get; set; }
        public int ApplicationNo {get; set;}
        public string PPNo {get; set;}
        public string CandidateName {get; set;}
        public List<ProcessReferredDto> Referrals {get; set;}
        
    }

    public class ProcessReferredDto
    {
        public string CustomerName {get; set;}
        public string CategoryRef {get; set;}
        public string DateReferred {get; set;}
        public string RefStatusDate {get; set; }
        public string CurrentStatus {get; set; }
        public List<ProcessItemDto> ProcessItems {get; set;}
    }
    public class ProcessItemDto
    {
        public int SrNo {get; set;}
        public string ProcessingDate { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}