using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.Emails;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    public class EmailController : BaseApiController
    {
        private readonly IEmailService _mailService;
        private readonly IMapper _mapper;
        public EmailController(IEmailService mailService, IMapper mapper)
        {
            _mapper = mapper;
            _mailService = mailService;
        }

        [HttpPost("sendMail")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> SendEmail(EmailModel email)
        {
            var emailVerified = _mapper.Map<EmailModel, EmailDto>(email);
            if (emailVerified == null) return BadRequest(new ApiResponse(400, "Bad email structure"));
            
            return await _mailService.SendEmail(email);
        }
        
        [ApiExplorerSettings(IgnoreApi = true)]
        private string ValidateEmail(EmailDto mailDto)
        {
            string sErr = "";

            if (mailDto.EmailFrom == "")  sErr = sErr + "FROM email address not defined";
            if (mailDto.EmailToList == null) sErr = sErr + ". To email address not defined";
            if (mailDto.MailSubject == null) sErr = sErr + ". Subject of mail not defined";
            if (mailDto.MailBody == null) sErr = sErr + ". mail body not defined";

            return sErr;
        }

    }
}