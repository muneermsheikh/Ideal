using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Emails
{
    public class Email
    {
        public Email()
        {
        }

        [Required, Display(Name = "Sender's name")]  
        public string ToName { get; set; }  
        [Required, Display(Name = "Sender's email"), EmailAddress]  
        public string ToEmail { get; set; }  
        [Display(Name = "CC"), EmailAddress]  
        public string CCmail {get; set; }
        [Display(Name = "BCC"), EmailAddress]  
        public string BCCmail {get; set; }

        [Required]  
        public string Subject { get; set; }  
        [Required]  
        public string MessageBody { get; set; }  
    }
}