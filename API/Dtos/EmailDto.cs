using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class EmailDto
    {
        [EmailAddress]
        public string EmailFrom { get; set; }
        [EmailAddress]
        public List<string> EmailToList {get; set;}
        [EmailAddress]
        public List<string> EmailCCList {get; set;}
        public List<string> EmailBCCList {get; set;}
        public string MailSubject {get; set;}
        public string MailBody {get; set;}
        public bool IsHTMLBody {get; set; }
        
        
    }

    

}