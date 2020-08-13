using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Emails
{
    public class EmailModel
    {
        public EmailModel()
        {
        }

        public EmailModel(string senderEmailAddress, List<string> toEmail, 
            List<string> cCmail, List<string> bCCmail, 
            string subject, string messageBody)
        {
            SenderEmailAddress = senderEmailAddress;
            ToEmailList = toEmail;
            CCmailList = cCmail;
            BCCmailList = bCCmail;
            Subject = subject;
            MessageBody = messageBody;
        }

        [Required, Display(Name = "Sender's Name")]
        public string SenderName {get; set; }
        [Required, Display(Name = "Sender's Email Address")]  
        public string SenderEmailAddress { get; set; }  
        [Required, Display(Name = "Sender's email"), EmailAddress]  
        public List<string> ToEmailList { get; set; }  
        [Display(Name = "CC"), EmailAddress]  
        public List<string> CCmailList {get; set; }
        [Display(Name = "BCC"), EmailAddress]  
        public List<string> BCCmailList {get; set; }

        [Required]  
        public string Subject { get; set; }  
        [Required]  
        public string MessageBody { get; set; }  
    }
}