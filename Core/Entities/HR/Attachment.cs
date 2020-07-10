using System;

namespace Core.Entities.HR
{
    public class Attachment: BaseEntity
    {
        public int CandidateId { get; set; }
        public string AttachmentType { get; set; }
        public string AttachmentDescription { get; set; }
        public DateTime UploadedOn { get; set; } =  DateTime.Now;
    }
}