using System.Net.Mail;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.Emails;

namespace Core.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailModel email);
    }
}