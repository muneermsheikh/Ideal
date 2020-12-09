using System;
using System.Linq.Expressions;
using Core.Entities.Emails;
using Core.Entities.Masters;

namespace Core.Specifications
{
    public class EmailSpecCount : BaseSpecification<EmailModel>
    {
        public EmailSpecCount(EmailParam param) 
        : base(x => 
                (
                   (string.IsNullOrEmpty(param.SenderEmailAddress) || 
                        x.SenderEmailAddress.ToLower().Contains(param.SenderEmailAddress)) &&
                    (string.IsNullOrEmpty(param.ToEmailAddress) ||
                        x.ToEmailList.ToLower().Contains(param.ToEmailAddress)) &&
                    (string.IsNullOrEmpty(param.Subject) ||
                        x.Subject.ToLower().Contains(param.Subject)) &&
                    ((string.IsNullOrEmpty(param.MessageType)) || 
                        x.MessageType.ToLower().Contains(param.MessageType)) &&
                    (!param.DateSent.HasValue || DateTime.Compare(
                        x.DateSent.Date, param.DateSent.Value.Date)==0) &&
                    (!param.Id.HasValue || x.Id==param.Id)
                ))
        {
        }
    }
}