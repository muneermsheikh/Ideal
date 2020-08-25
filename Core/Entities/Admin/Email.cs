using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Enumerations;

namespace Core.Entities.Admin
{
    public class Email
    {
        public Email()
        {
        }

        public Email(enumTaskType emailType, int customerOrCandidateId, string emailFrom, 
            List<string> emailToList, List<string> emailCCList, List<string> emailBCCList, 
            string mailSubject, string mailBody, bool isHTML)
        {
            EmailType = emailType;
            CustomerOrCandidateId = customerOrCandidateId;
            EmailFrom = emailFrom;
            EmailToList = emailToList;
            EmailCCList = emailCCList;
            EmailBCCList = emailBCCList;
            MailSubject = mailSubject;
            MailBody = mailBody;
            IsHTML = isHTML;
        }

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