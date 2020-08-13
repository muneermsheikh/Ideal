using System.Net.Mail;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.Emails;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public async Task<bool> SendEmail(EmailModel email)
        {
            
            var message = new MailMessage();  
            message.From = new MailAddress(email.SenderEmailAddress);
            
            foreach(var item in email.ToEmailList)
            {
                message.To.Add(new MailAddress(item.ToString()));
            }
            
            foreach(var item in email.CCmailList)
            {
                message.CC.Add(new MailAddress(item.ToString()));
            }

            foreach(var item in email.BCCmailList)
            {
                message.Bcc.Add(new MailAddress(item.ToString()));
            }
            message.Subject = email.Subject;
            message.Body = createEmailBody(email.Subject.ToString());

            message.IsBodyHtml = true;  

            using (var smtp = new SmtpClient())  
            {  
                await smtp.SendMailAsync(message);  
                await Task.FromResult(0);  
            }  

            return true;
        }

           private string createEmailBody(string message)  
        {  
            string body = string.Empty;  
            /*
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/htmlTemplate.html")))  
            {  
                body = reader.ReadToEnd();  
            } 
            */ 
            //body = body.Replace("{UserName}", userName);  
            body = body.Replace("{message}", message);  
            return body;  
        }  
    }
}