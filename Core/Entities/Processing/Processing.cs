using System;
using Core.Enumerations;

namespace Core.Entities.Processing
{
     public class Processing: BaseEntity
    {
        public int CVRefId { get; set; }
        public DateTime ProcessingDate { get; set; }
        public enumProcessingStatus Status { get; set; }
        public int? NextProcessingId { get; set; }
        public string Remarks { get; set; }
    }
}