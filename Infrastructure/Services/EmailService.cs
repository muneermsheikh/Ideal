using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.Emails;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Masters;
using Core.Enumerations;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly ATSContext _context;
        public EmailService(ATSContext context)
        {
            _context = context;
        }

        public async Task<bool> SendEmail(EmailModel email)
        {

            var message = new MailMessage();
            message.From = new MailAddress(email.SenderEmailAddress);

            foreach (var item in email.ToEmailList)
            {
                message.To.Add(new MailAddress(item.ToString()));
            }

            foreach (var item in email.ccEmailList)
            {
                message.CC.Add(new MailAddress(item.ToString()));
            }

            foreach (var item in email.bccEmailList)
            {
                message.Bcc.Add(new MailAddress(item.ToString()));
            }
            message.Subject = email.Subject;

            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                await smtp.SendMailAsync(message);
                await Task.FromResult(0);
            }

            return true;
        }

        public async Task<EmailModel> SendCVForwardingMessage(EmailModel mailModel,
            string AddresseeBody, CVForwardMessages messages)
        {
            string mailBody = ComposeCVForwardingMessageBody(messages);

            mailModel.MessageBody = messages.DateForwarded.Date + "<br><br>" + AddresseeBody + "Dear Sir<br><br>" +
                mailBody;

            var mailSent = await SendEmail(mailModel);
            return mailModel;
        }

        public async Task<EmailModel> GetEmail(int emailId)
        {
            var mail = await _context.Emails.Where(x => x.Id == emailId).SingleOrDefaultAsync();
            if (mail == null) {return null; }
            return mail;
        }

        public async Task<EmailModel> GetEmailForDLAcknowledgement(int enquiryId)
        {
            var mail = await _context.Emails.Where(x => x.RefNo == enquiryId.ToString() && x.MessageType == "dlacknowledgement")
                .SingleOrDefaultAsync();
            
            return mail;
            
        }

        private string ComposeCVForwardingMessageBody(CVForwardMessages message)
        {
            string htmlBody = "";
            var candidates = message.candidates;

            htmlBody = "We are pleased to forward herewith Candidate Profile(s) as stated " +
                "below against requirement mentioned:<br><br><b>Your requirement Reference</b><br>" +
                "<table>" + //start of table
                    "<th>Yr Requirement<br>dated</th><th>Our Reference</th>Category</th>" + //<th> is tableheader tag
                    "<td>" + message.EnquiryDate + "</td><td>" + message.EnquiryNo + "</td><td>" +
                    message.CategoryRef + "</td>" +
                "</table>" +    // end of table
                    "<br><br><b>Candidate(s) referred:</b><br>" +
                "<table>" +
                    "<tr>" +
                        "<th>Sr<br>No</th><th>Application<br>No</th><th>Candidate Name<tb></th>" +
                        "<th>PP No</th><th>Aadhar No</th><th>";
            if (message.includeGrade) htmlBody += "Grade </th><th>";
            if (message.includePhoto) htmlBody += "Photograph</th><th>";
            if (message.includeSalary) htmlBody += "Salary Expectation</th><th>";
            htmlBody += "</th><th>Remarks</th>" +
        "</tr>";
            foreach (var cand in candidates)
            {
                htmlBody += "<tr></td><td>" + cand.SrNo + "</td><td>" + cand.ApplicationNo + "</td><td>" +
                    cand.CandidateName + "</td><td>" + cand.PPNo + "</td><td>" +
                    cand.AadharNo + "</td><td>";
                if (message.includeGrade) htmlBody += cand.Grade + "</td><td>";
                if (message.includePhoto) htmlBody += cand.PhotoUrl + "</td><td>";
                if (message.includeSalary) htmlBody += cand.SalaryExpectation + "</td><td>";
                htmlBody += cand.Remarks + "</td<td></tr>";
            }
            htmlBody +=
        "</table>" +
        "<br><br>Kindly acknowledge receipt and let us have your selection decision at the earliest" +
        "<br><br>Early selection decisions help us to retain the candidate's interest; therefore, we " +
        "request you to kindly intimate us your selection deicsion at the earliest<br>br>" +
        "Best regards<br><br>" + message.SenderName + "<br>" + message.SenderDesignation +
        "<br>Phone" + message.SenderPhone + "<br>end of message";
            return htmlBody;
        }

        public async Task<IReadOnlyList<EmailModel>> GetEmails()
        {
            return await _context.Emails.OrderBy(x => x.DateSent).ToListAsync();
        }

        public Task<EmailModel> GetEmailOfAcknowledgement(int enquiryId)
        {
            throw new System.NotImplementedException();
        }
    }
}