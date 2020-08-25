using System;

namespace Core.Entities.HR
{
    public class Attachment: BaseEntity
    {
        public Attachment()
        {
        }

        public int CandidateId { get; set; }
        public string AttachmentType { get; set; }
        public string AttachmentDescription { get; set; }
        public string AttachmentUrl {get; set; }
        public DateTime UploadedOn { get; set; } =  DateTime.Now;
    }
}