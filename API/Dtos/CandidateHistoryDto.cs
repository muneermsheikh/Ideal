using System.Collections.Generic;

namespace API.Dtos
{
    public class CandidateHistoryDto
    {
        public string CandidateName {get; set;}
        public string CustomerName {get; set;}
        public string CategoryRef {get; set;}
        public string DateReferred {get; set;}
        public string RefStatusDate {get; set; }
        public string CurrentStatus {get; set; }
        public List<HistoryItemDto> HistoryItems {get; set;}
    }
    
    public class HistoryItemDto
    {
        public int SrNo {get; set;}
        public string ProcessingDate { get; set; }
        public string StatusName { get; set; }
        public string NextStatusName { get; set; }
    }
}