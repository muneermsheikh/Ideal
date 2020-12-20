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

using System.Web;
using System.IO;
using System.Net.Mime;
using System.Text;
using System;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly ATSContext _context;
        private readonly IConfiguration _config;
        private readonly ICategoryService _catService;
        private readonly ICustomerService _custService;
        private readonly IEmployeeService _empService;
        public EmailService(ATSContext context, IConfiguration config, 
            ICategoryService catService, ICustomerService custService,
            IEmployeeService empService)
        {
            _custService = custService;
            _empService = empService;
            _catService = catService;
            _config = config;
            _context = context;
        }

        public async Task<bool> SendEmail(EmailModel email)
        {


            var message = new MailMessage();
            message.From = new MailAddress(email.SenderEmailAddress);

            message.To.Add(new MailAddress(email.ToEmailList));
            if (string.IsNullOrEmpty(email.ccEmailList)) message.CC.Add(new MailAddress(email.ccEmailList));
            if (string.IsNullOrEmpty(email.bccEmailList)) message.Bcc.Add(new MailAddress(email.bccEmailList));
            /*
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
            */
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
            if (mail == null) { return null; }
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

        
        public async Task<int> ComposeHRTaskAssignmentMessageBody(List<int> enquiryItemIds, 
            int enquiryNo, DateTime enquiryDate, int customerId, int projManagerId)
        {
            var emails = new List<EmailModel>();
            string htmlBody = "";
            var enquiryItems = await _context.EnquiryItems.Where(x => enquiryItemIds.Contains(x.Id))
                .Include(x => x.Remuneration)
                .Include(x => x.JobDesc)
                .Include(x => x.TasksAssigned)
                .OrderBy(x => x.HRExecutiveId)
                .ToListAsync();
            
            if (enquiryItems == null) return 0; 
            var SalaryCurrency = await _custService.CustomerCountryCurrency(customerId);
            var projManager = await _empService.GetEmployeeByIdAsync(projManagerId);
            string ProjectManagerTitle = projManager.Gender == "M" ? "Mr. " : "Ms. ";
            string ProjectManagerName=projManager.FullName;
            string ProjectManagerDesignation=projManager.Designation;
            string ProjectManagerEmail=projManager.Email;
            string ProjectManagerMobile=projManager.Mobile;

            string ExecTitle = "";
            string ExecName="";
            string ExecDesignation="";
            string ExecEmail="";
            string ExecMobile="";
            string ccEmail="";
            string bccEmail="";

            int EmailsSentCount = 0;
            int HRExecId = Convert.ToInt32(enquiryItems[0].HRExecutiveId);
            string TableHeader = "<br><br>Following task is assigned to you:<br><br><b>Requirements assigned:</b><br>" +
                "<table><th>Reference</th><th>Category</th><th>Quantity</th>CVs required</th>" + 
                "<th>ECNR</th><th>Assess<br>Reqd</th><th>Salary</th>"+
                "<th>Accomm</<th><th>Food</th><th>Transport</th><th>Other</th>"+
                "<th>DL Date<br>Complete by</th><th>Remarks</th>";
            string s="";

            foreach(var item in enquiryItems)
            {
                if (item.HRExecutiveId != HRExecId)
                {
                    if (HRExecId != 0) {
                        htmlBody += "</tr></table><br>" +
                            "<li>if job description is not available at the given url link, check with your " + 
                            "Supervisor</li>" +
                            "<li>For any query, contact with your Supervisor immediately<br><br>"+
                            "Best regards<br><br>" + ProjectManagerName + "<br>" + ProjectManagerDesignation +
                            "<br>Phone" + projManager.Mobile + "<br>end of message";

                        if (await SendEmail (new EmailModel(ProjectManagerEmail, ExecEmail, ccEmail, bccEmail, 
                            "Task Assignment for DL Number " + enquiryNo,htmlBody))) ++EmailsSentCount;
                    }

                    HRExecId = Convert.ToInt32(item.HRExecutiveId);
                    var hrExec = await _empService.GetEmployeeByIdAsync(HRExecId);
                    ExecTitle = hrExec.Gender == "M" ? "Mr. " : "Ms. ";
                    ExecName=hrExec.FullName;
                    ExecDesignation=hrExec.Designation ?? "";
                    ExecEmail=hrExec.Email;
                    ExecMobile=hrExec.Mobile;
                    htmlBody = DateTime.Today + "<br><br>" + ExecTitle + ExecName + "<br>" + 
                        ExecDesignation + TableHeader;
                }
                htmlBody += "<tr><td>"+enquiryNo + "-" + item.SrNo + "</td>" +
                    "<td>" + _catService.GetCategoryNameFromCategoryId(item.CategoryItemId) + "</td>" +
                    "<td>" + item.Quantity + "</td><td>" + item.MaxCVsToSend + "</td>";
                if ((bool)item.Ecnr) { htmlBody += "<td>Yes</td>"; } else {htmlBody += "<td>No</td>"; } 
                if ((bool)item.AssessmentReqd) { htmlBody += "<td>Yes</td>"; } else {htmlBody += "<td>No</td>"; } 
                if (item.Remuneration != null)
                {
                    htmlBody += "<td>" + SalaryCurrency + item.Remuneration.SalaryMin;
                    s = item.Remuneration.SalaryMax > 0 ? " to " + item.Remuneration.SalaryMax : "";
                    htmlBody += "</td><td>" + s;
                    s = item.Remuneration.HousingAllowance > 0 ? item.Remuneration.HousingAllowance.ToString() : item.Remuneration.Housing ? "Free" : "Not Provided";
                    htmlBody += "</td><td>" + s;
                    s = item.Remuneration.FoodAllowance > 0 ? item.Remuneration.FoodAllowance.ToString() : item.Remuneration.Food ? "Free" : "Not Provided";
                    htmlBody += "</td><td>" + s;
                    s = item.Remuneration.TransportAllowance > 0 ? item.Remuneration.TransportAllowance.ToString() : item.Remuneration.Transport ? "Free" : "Not Provided";;
                    htmlBody += "</td><td>" + s + "</td><td>" + item.Remuneration.OtherAllowance.ToString() +"</td>";
                } else {
                    htmlBody +="<td></td><td></td><td></td><td></td><td></td>";
                }
                
                s="";
                if (item.TasksAssigned !=null) {
                    foreach(var t in item.TasksAssigned)
                    {
                        if (t.TaskType.ToLower() == "hr" && t.TaskStatus.ToLower() != "completed")
                        {
                            s = t.TaskDate + "<br>" + t.CompleteBy.Date;
                        }
                        else if (s != "") 
                        {
                            break;
                        }
                    } 
                }
                htmlBody += "<td>" + s +"<td>";

                if (item.JobDesc !=null) {htmlBody += item.JobDesc.JobProfileUrl ?? ""; }
                htmlBody +="</td>";
            }
            htmlBody += "</tr></table><br>" +
                "<li>if job description is not available at the given url link, check with your Supervisor</li>" +
                "<li>For any query, contact with your Supervisor immediately<br><br>"+
                "Best regards<br><br>" + ProjectManagerName + "<br>" + ProjectManagerDesignation +
                "<br>Phone" + ProjectManagerMobile + "<br>end of message";
            var b = SendEmail(new EmailModel(ProjectManagerEmail, ExecEmail, ccEmail, bccEmail, 
                      "Task Assignment for DL Number " + enquiryNo,htmlBody));
            if (Convert.ToBoolean(b))  ++EmailsSentCount;
            return EmailsSentCount;
        }

        
    }


}