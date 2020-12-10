using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class EmailDto
    {
        public string refNo {get; set;}
        [EmailAddress]
        public string EmailFrom { get; set; }
        [EmailAddress]
        public string EmailToList {get; set;}
        public string EmailCCList {get; set;}
        public string EmailBCCList {get; set;}
        public string MailSubject {get; set;}
        public string MailBody {get; set;}
        public bool IsHTMLBody {get; set; }
    }
}
