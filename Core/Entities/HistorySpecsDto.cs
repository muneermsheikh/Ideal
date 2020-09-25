using System;

namespace Core.Entities
{
    public class HistorySpecsDto
    {
        public int[] enquiryItemIds {get; set;}
        public int[] enquiryIds {get; set;}
        public int[] candidateIds {get; set;}
        public int[] customerIds {get; set;}
        public DateTime FromDate {get; set;}
        public DateTime UptoDate {get; set;}
        
    }
}