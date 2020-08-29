using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.HR;
using Core.Entities.Identity;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class CVEvaluationController : BaseApiController
    {
        private readonly ATSContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICVEvaluationService _evalService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICVRefService _cvrefService;
        
        public CVEvaluationController(ATSContext context, IMapper mapper, 
            UserManager<AppUser> userManager, ICVEvaluationService evalService, 
            IUnitOfWork unitOfWork, ICVRefService cvrefService)
        {
            _cvrefService = cvrefService;
            _unitOfWork = unitOfWork;
            _evalService = evalService;
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
        }
        
//Forward to clients
        [HttpPost("refcvstoclient")]
        public async Task<CVForward> ForwardCVsToClient(CVForward cvforward)
        {
            var cvsReferred = await _cvrefService.ReferCVsToForward(cvforward);

            return cvsReferred;
        }
//evaluation
        
        [HttpGet("getevalbyid/{id}")]
        public async Task<ActionResult<CVEvaluationDto>> GetCVEvaluationById (int id)
        {
            var eval = await _evalService.GetCVEvaluationByIdAsync(id);
            if(eval==null) return NotFound(new ApiResponse(400, "Not found"));
            return Ok(_mapper.Map<CVEvaluation, CVEvaluationDto>(eval));
}
        
        [HttpGet("getcveval")]
        public async Task<ActionResult<CVEvaluation>> GetCVEvaluation(int candidateId, int enquiryItemId)
        {
            return await _evalService.GetCVEvaluation(candidateId, enquiryItemId);
        }

        [HttpPost("submitCVtoSup")]
        public async Task<ActionResult<CVEvaluationDto>> SubmitCVToSup(int candidateId, int enquiryItemId)
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(HttpContext.User);
            if(user==null) return Unauthorized(new ApiResponse(400, "Unauthorized access"));
            var userId =  await _context.Employees.Where(x => x.Email == user.Email).Select(x => x.Id).FirstOrDefaultAsync();
            if (userId==0) return Unauthorized(new ApiResponse(400, "Unauthorized access"));
            var eval = await _evalService.CVSubmitToSup(candidateId, enquiryItemId, userId);
            return Ok(_mapper.Map<CVEvaluation, CVEvaluationDto>(eval));
        }

        [HttpPost("evalbySup")]
        public async Task<ActionResult<CVEvaluationDto>> CVEvaluationBySup(int cvEvalId, 
            enumItemReviewStatus status)
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(HttpContext.User);
            if(user == null) return Unauthorized(new ApiResponse(400, "Unauthorized access"));

            var userId = await _context.Employees.Where(x => x.Email == user.Email).Select(x => x.Id).FirstOrDefaultAsync();
            if (userId == 0) return Unauthorized(new ApiResponse(400, "Unauthorized access"));
            
            var eval =  await _evalService.CVEvalBySup(cvEvalId, status, userId);
            if (eval==null) return BadRequest(new ApiResponse(400, "Failed to update the evaluation"));
            return Ok(_mapper.Map<CVEvaluation, CVEvaluationDto>(eval));
        }


        [HttpPut("evalbyHRM")]
        public async Task<ActionResult<CVEvaluationDto>> CVEvaluationByHRM(int cvEvalId, enumItemReviewStatus status)
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(HttpContext.User);
            var userId = await _context.Employees.Where(x => x.Email == user.Email).Select(x => x.Id).FirstOrDefaultAsync();

            var cvEvaluated = await _evalService.CVEvalByHRM(cvEvalId, status, userId);

            return _mapper.Map<CVEvaluation, CVEvaluationDto>(cvEvaluated);
        }

        [HttpGet("pendingevalofloggedinuser")]
        public async Task<ActionResult<IReadOnlyList<CVEvaluationDto>>> PendingEvaluationsOfLoggedInUser()
        {
            var user =  await _userManager.FindByEmailFromClaimsPrincipal(HttpContext.User);
            if (user==null) return Unauthorized(new ApiResponse(400, "Unauthorized access"));
            var userId = await _context.Employees.Where(x => x.Email == user.Email).Select(x => x.Id).FirstOrDefaultAsync();
            var lst = await _evalService.GetPendingEvaluationOfAUser(userId);
            if (lst == null || lst.Count == 0) return NotFound(new ApiResponse(400, "The logged in user " +
                  user.DisplayName + " has no pending evaluations to review"));
            return Ok(_mapper.Map<IReadOnlyList<CVEvaluation>, IReadOnlyList<CVEvaluationDto>>(lst));
        }

        [HttpGet("pendingevalofauser")]
        public async Task<ActionResult<IReadOnlyList<CVEvaluationDto>>> PendingEvaluationsOfAUser(int userId)
        {
            var user = await _context.Employees.Where(x => x.Id == userId && x.IsInEmployment == true)
                .Select(x => x.KnownAs).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(user)) return NotFound(new ApiResponse(400, "Invalid employee Id"));
            var lst = await _evalService.GetPendingEvaluationOfAUser(userId);
            if (lst == null || lst.Count == 0) return NotFound(new ApiResponse(400, "The user " +
                    user + " has no pending evaluations to review"));
            return Ok(_mapper.Map<IReadOnlyList<CVEvaluation>, IReadOnlyList<CVEvaluationDto>>(lst));
        }
        
    }
}