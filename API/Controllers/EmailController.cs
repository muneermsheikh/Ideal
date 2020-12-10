using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.Emails;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    public class EmailController : BaseApiController
    {
        private readonly IEmailService _mailService;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<EmailModel> _repoMail;
        public EmailController(IEmailService mailService, IMapper mapper, IGenericRepository<EmailModel> repoMail)
        {
            _repoMail = repoMail;
            _mapper = mapper;
            _mailService = mailService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmailModel>> GetEmail(int emailId)
        {
            var email = await _mailService.GetEmail(emailId);
            return Ok(email);
        }

        [HttpGet("dlackn/{enquiryId}")]
        public async Task<ActionResult<EmailModel>> GetEmailOfAcknowledgement(int enquiryId)
        {
            var email = await _mailService.GetEmailOfAcknowledgement(enquiryId);
            
            return Ok(email);
        }

        [HttpGet("emailindex")]
        public async Task<ActionResult<Pagination<MailIndexDto>>> GetEmails(EmailParam param)
        {
            var spec = new EmailSpecs(param);
            var countSpec = new EmailSpecCount(param);
            var totalItems = await _repoMail.CountWithSpecAsync(countSpec);

            var mails = await _repoMail.ListWithSpecAsync(spec);
            if (mails == null) return BadRequest(new ApiResponse(404));

            var indexToReturn = new List<MailIndexDto>();
            foreach(var m in mails)
            {
                indexToReturn.Add(new MailIndexDto {
                    SenderEmailAddress = m.SenderEmailAddress,
                    ToEmailList = m.ToEmailList,
                    Subject = m.Subject,
                    DateSent = m.DateSent,
                    MessageType = m.MessageType
                });
            }
            return Ok(new Pagination<MailIndexDto>(param.PageIndex,
                param.PageSize, totalItems, indexToReturn));
        }

        [HttpPost("sendMail")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> SendEmail(EmailModel email)
        {
            // var emailVerified = _mapper.Map<EmailModel, EmailDto>(email);
            // if (emailVerified == null) return BadRequest(new ApiResponse(400, "Bad email structure"));

            return await _mailService.SendEmail(email);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private string ValidateEmail(EmailDto mailDto)
        {
            string sErr = "";

            if (mailDto.EmailFrom == "") sErr = sErr + "FROM email address not defined";
            if (mailDto.EmailToList == null) sErr = sErr + ". To email address not defined";
            if (mailDto.MailSubject == null) sErr = sErr + ". Subject of mail not defined";
            if (mailDto.MailBody == null) sErr = sErr + ". mail body not defined";

            return sErr;
        }

    }
}