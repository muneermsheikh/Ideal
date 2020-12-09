using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.Emails;
using Core.Entities.Masters;

namespace Core.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailModel email);
        Task<EmailModel> SendCVForwardingMessage(EmailModel mailModel, 
            string AddresseeBody, CVForwardMessages messages);
        Task<EmailModel> GetEmail(int emailId);
        Task<EmailModel> GetEmailOfAcknowledgement (int enquiryId);
        Task<IReadOnlyList<EmailModel>> GetEmails();
    }
}