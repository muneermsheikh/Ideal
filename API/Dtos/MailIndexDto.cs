using System;

namespace API.Dtos
{
    public class MailIndexDto
    {
        public string SenderEmailAddress { get; set; }  
        public string ToEmailList { get; set; }  
        public DateTime DateSent {get; set; }
        public string MessageType {get; set; }
        public string Subject { get; set; }  
    }
}