using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Emails
{
    public class EmailModel: BaseEntity
    {
        public EmailModel()
        {
        }

        public EmailModel(string senderEmailAddress, string toEmail, 
            string cCmail, string bCCmail, 
            string subject, string messageBody)
        {
            SenderEmailAddress = senderEmailAddress;
            ToEmailList = toEmail;
            ccEmailList = cCmail;
            bccEmailList = bCCmail;
            Subject = subject;
            MessageBody = messageBody;
        }

        [Required, Display(Name = "Sender's Name")]
        public string SenderName {get; set; }

        [Required, Display(Name = "Sender's Email Address")]  
        public string SenderEmailAddress { get; set; }  
        public string RefNo {get; set; }

        [Required, Display(Name = "Sender's email"), EmailAddress]  
        public string ToEmailList { get; set; }  

        [Display(Name = "CC"), EmailAddress]  
        public string ccEmailList {get; set; }

        [Display(Name = "BCC"), EmailAddress]  
        public string bccEmailList {get; set; }

        public DateTime DateSent {get; set; }
        public string MessageType {get; set; }

        [Required]  
        public string Subject { get; set; }  
        [Required]  
        public string MessageBody { get; set; }  
    }
}