using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.HR;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class HRController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IGenericRepository<HRSkillClaim> _skillRepo;

        private readonly IInternalHRService _internalHrService;

        private readonly ICVEvaluationService _cvEvalService;

        private readonly UserManager<AppUser> _userManager;

        private readonly IHRService _hrService;

        public HRController(
            IUnitOfWork unitOfWork,
            UserManager<AppUser> userManager,
            IGenericRepository<HRSkillClaim> skillRepo,
            IMapper mapper,
            IInternalHRService internalHrService,
            ICVEvaluationService cvEvalService,
            IHRService hrService
        )
        {
            _hrService = hrService;
            _cvEvalService = cvEvalService;
            _internalHrService = internalHrService;
            _skillRepo = skillRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet("skills")]
        public async Task<ActionResult<Pagination<HRSkillClaimsDto>>>
        GetEmpSkilledForACategory([FromQuery] HRSkillClaimsParam hParam)
        {
            if (hParam.PageIndex == 0)
                return BadRequest(new ApiResponse(400,
                    " Pagination not defined"));

            var spec = new HRSkillClaimsSpecs(hParam);
            var countSpec = new HRSkillClaimsWithFiltersForCountSpec(hParam);
            var totalItems = await _skillRepo.CountWithSpecAsync(countSpec);

            var listClaims = await _skillRepo.GetEntityListWithSpec(spec);
            if (listClaims == null)
                return NotFound(new ApiResponse(400,
                    "No employees found with requested skills"));

            var data =
                _mapper
                    .Map
                    <IReadOnlyList<HRSkillClaim>,
                        IReadOnlyList<HRSkillClaimsDto>
                    >(listClaims);

            return Ok(new Pagination<HRSkillClaimsDto>(hParam.PageIndex,
                hParam.PageSize,
                totalItems,
                data));
        }

        [HttpPost("registerSkills")]
        public async Task<ActionResult<HRSkillClaimsDto>>
        RegisterHREmployeeSkills(HRSkillClaim hrskills)
        {
            var sk = await _internalHrService.AddEmployeeHRSkill(hrskills);
            if (sk == null)
                return NotFound(new ApiResponse(400,
                    "failed to register the employee skill"));

            _mapper.Map<HRSkillClaim, HRSkillClaimsDto> (sk);

            return Ok(sk);
        }

        [HttpPut]
        public async Task<ActionResult<HRSkillClaimsDto>>
        UpdateEmployeeSkills(HRSkillClaim hrskills)
        {
            var sk = await _internalHrService.UpdateEmployeeHRSkill(hrskills);
            if (sk == null)
                return BadRequest(new ApiResponse(400,
                    "failed to update the employee skill"));

            _mapper.Map<HRSkillClaim, HRSkillClaimsDto> (sk);

            return Ok(sk);
        }

        [HttpDelete]
        public async Task<int> DeleteHREmployeeSkills(HRSkillClaim hrskills)
        {
            return await _internalHrService.DeleteEmployeeHRSkill(hrskills);
        }

        [HttpPost("assignTaskToHRExecutive")]
        public async Task<ToDo> AssignTaskToHRExecutive(ToDo toDo)
        {
            return await _unitOfWork.Repository<ToDo>().AddAsync(toDo);
        }

        [HttpPost("editHRExecTask")]
        public async Task<ToDo> EditHRExecutiveTask(ToDo todo)
        {
            var t = await _unitOfWork.Repository<ToDo>().UpdateAsync(todo);
            if (t == null) return null;
            return t;
        }

        [HttpDelete("deleteHRExecutiveTask")]
        public async Task<int> DeleteHRExecutiveTask(ToDo toDo)
        {
            return await _unitOfWork.Repository<ToDo>().DeleteAsync(toDo);
        }

        [HttpPost("submitCVtoSupervisor")]
        public async Task<ActionResult<CVEvaluation>>
        SubmitCVToSupervisorForEvaluation(CVEvaluation cvEval)
        {
            var eval = await _cvEvalService.CVForEvaluation_ByHRSup(cvEval);
            if (eval == null)
                return BadRequest(new ApiResponse(404,
                    "failed to register supervisor evaluation of the CV"));
            return eval;
        }

        [HttpGet("assessmentsheet")]
        public async Task<Assessment> GetCandidateAssessmentSheet(
            int CandidateId, int EnquiryItemId)
        {
            var user =
                await _userManager
                    .FindByEmailFromClaimsPrincipal(HttpContext.User);
            int employeeId = user.EmployeeId;
            string employeeDisplayName = user.DisplayName;

            return await _hrService
                .GetCandidateAssessmentSheet(CandidateId,
                EnquiryItemId,
                employeeId,
                employeeDisplayName);
        }
        
        [ApiExplorerSettings(IgnoreApi = true)]
        private string ValidateAssessment(Assessment assessment)
        {
            string errorstring = "";

            if (assessment.CandidateId == 0)
                errorstring = "Candidate not defined";
            if (string.IsNullOrEmpty(assessment.CategoryNameAndRef))
                errorstring += "Category of assessment not defined";
            if (string.IsNullOrEmpty(assessment.CustomerNameAndCity))
                errorstring += "Cusomer name and city not defined";

            foreach (var q in assessment.AssessmentItems)
            {
                if (q.IsMandatory && q.Assessed != true)
                    errorstring += "Mmandatory question not assessed";
                if (q.PointsAllotted > q.MaxPoints)
                    errorstring +=
                        "Marks alloted for question No." +
                        q.QuestionNo +
                        " more than maxm points";
                if (q.PointsAllotted.HasValue & q.Assessed != true)
                    q.Assessed = true;
            }
            return errorstring;
        }
    }
}
