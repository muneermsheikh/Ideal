using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities.Emails;
using Core.Entities.Masters;

namespace Core.Specifications
{
    public class EmailSpecs : BaseSpecification<EmailModel>
    {
        public EmailSpecs(EmailParam param) 
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

            ApplyPaging(param.PageSize * (param.PageIndex-1), param.PageSize);

            if (!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort)
                {
                    case "DateSentAsc":
                        AddOrderBy(x => x.DateSent);
                        break;
                    case "DateSentDesc":
                        AddOrderByDescending(x => x.DateSent);
                        break;
                    case "SenderEmailAddressAsc":
                        AddOrderBy(x => x.SenderEmailAddress);
                        break;
                    case "SenderEmailAddressDesc":
                        AddOrderByDescending(x => x.SenderEmailAddress);
                        break;
                    case "ToEmailAddressAsc":
                        AddOrderBy(x => x.ToEmailList);
                        break;
                    case "ToEmailAddressDesc":
                        AddOrderByDescending(x => x.ToEmailList);
                        break;
                    case "Subject":
                        AddOrderBy(x => x.Subject);
                        break;
                    default:
                        AddOrderByDescending(x => x.DateSent);
                        break;
                }
            }

        }

        public EmailSpecs(int Id) 
            : base(x => x.Id == Id)
        {
        }
    }
}