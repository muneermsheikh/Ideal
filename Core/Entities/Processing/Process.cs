using System;
using Core.Enumerations;

namespace Core.Entities.Processing
{
     public class Process: BaseEntity
    {
        public Process()
        {
        }

        public Process(int cVRefId, DateTime processingDate, enumProcessingStatus status, string remarks, enumProcessingStatus? nextProcessingId)
        {
            CVRefId = cVRefId;
            ProcessingDate = processingDate;
            Status = status;
            NextProcessingId = nextProcessingId;
            Remarks = remarks;
        }

        public int CVRefId { get; set; }
        public DateTime ProcessingDate { get; set; }
        public enumProcessingStatus Status { get; set; }
        public enumProcessingStatus? NextProcessingId { get; set; }
        public string attachmentUrl {get; set;}
        public virtual Travel Travel {get; set;}
        public string Remarks { get; set; }
    }
}