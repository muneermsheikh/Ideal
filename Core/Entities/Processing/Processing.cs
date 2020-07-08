using System;

namespace Core.Entities.Processing
{
     public class Processing: BaseEntity
    {
        public int CVRefId { get; set; }
        public DateTime ProcessingDate { get; set; }
        public ProcessingStatus Status { get; set; }
        public int? NextProcessingId { get; set; }
        public string Remarks { get; set; }
    }
}