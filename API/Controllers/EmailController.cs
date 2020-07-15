using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    public class EmailController: BaseApiController
    {
        [HttpPost("sendemail")]  
        public async Task SendEmail([FromBody]JObject objData)  
        {  
            var message = new MailMessage();  
            message.To.Add(new MailAddress(objData["ToName"].ToString() + " <" + objData["toemail"].ToString() + ">"));  
            message.From = new MailAddress("Amit Mohanty <amitmohanty@email.com>");  
            message.Bcc.Add(new MailAddress("Amit Mohanty <amitmohanty@email.com>"));  
            message.Subject = objData["Subject"].ToString();  
            message.Body = createEmailBody(objData["ToName"].ToString(), objData["MessageBody"].ToString());  
            message.IsBodyHtml = true;  
            using (var smtp = new SmtpClient())  
            {  
                await smtp.SendMailAsync(message);  
                await Task.FromResult(0);  
            }  
        }  
  
        
        private string createEmailBody(string userName, string message)  
        {  
            string body = string.Empty;  
            /*
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/htmlTemplate.html")))  
            {  
                body = reader.ReadToEnd();  
            } 
            */ 
            body = body.Replace("{UserName}", userName);  
            body = body.Replace("{message}", message);  
            return body;  
        }  
    }
}