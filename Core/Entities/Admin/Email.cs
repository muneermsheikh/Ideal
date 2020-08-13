using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Enumerations;

namespace Core.Entities.Admin
{
    public class Email
    {
        public enumTaskType EmailType {get; set;}
        public int CustomerOrCandidateId {get; set;}
        public string EmailFrom { get; set; }
        public List<string> EmailToList {get; set;}
        public List<string> EmailCCList {get; set;}
        public List<string> EmailBCCList {get; set;}
        public string MailSubject {get; set;}
        public string MailBody {get; set;}
        public bool IsHTML {get; set;}
    }
}