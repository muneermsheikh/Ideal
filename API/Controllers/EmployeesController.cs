using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities.HR;
using Core.Entities.Masters;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EmployeesController : BaseApiController
    {
        private readonly IEmployeeService _empService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesController(IEmployeeService empService, IMapper mapper,
        IGenericRepository<Employee> repoEmp, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _empService = empService;
        }
/*
        [HttpGet("aadharNo/{id}")]
        public async Task<EmployeeToReturnDto> GetEmployeeByName(string aadharNo)
        {
          
            var emp = await _empService.GetEmployeeByIdAsync(aadharNo);
            if (emp == null) return NotFound(new ApiResponse(404));
            var empDto = _mapper.Map<Employee, EmployeeToReturnDto>(emp);
            return empDto;
          
            return null;
        }
*/
        [HttpGet("{EmployeeId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeToReturnDto>> GetEmployeeById(int EmployeeId)
        {
            var emp = await _empService.GetEmployeeByIdAsync(EmployeeId);
            if (emp == null) return NotFound(new ApiResponse(404));
            var empDto = _mapper.Map<Employee, EmployeeToReturnDto>(emp);
            return empDto;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Pagination<IReadOnlyList<EmployeeToReturnDto>>>> GetEmployeeList(
            [FromQuery] EmployeeParam empParam)
        {
            var empRepo = _unitOfWork.Repository<Employee>();
            var spec = new EmployeeSpecs(empParam);
            var specCount = new EmployeeSpecWithCount(empParam);
            var totalItems = await empRepo.CountWithSpecAsync(specCount);

            var emps = await empRepo.ListWithSpecAsync(spec);
            if (emps == null) return NotFound(new ApiResponse(404,
                "No employees found matching the criteria"));

            var data = _mapper
                .Map<IReadOnlyList<Employee>, IReadOnlyList<EmployeeToReturnDto>>(emps);

            return Ok(new Pagination<EmployeeToReturnDto>
                    (empParam.PageIndex, empParam.PageSize, totalItems, data));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeToReturnDto>> CreateNewEmployeeAsync(EmployeeToAddDto emp)
        {
            var empToAdd = await _empService.CreateNewEmployeeAsync(emp.Gender, emp.FirstName,
                emp.SecondName, emp.FamilyName, emp.KnownAs, emp.Address1, emp.Address2,
                emp.City, emp.PIN, emp.District, emp.State, "India", emp.Mobile, emp.Email,
                emp.AadharNo, emp.PassportNo, emp.Designation, emp.DateOfBirth, emp.DateOfJoining);

            if (empToAdd == null) return BadRequest(new ApiResponse(400));

            var empAdded = _mapper.Map<Employee, EmployeeToReturnDto>(empToAdd);
            return empAdded;
        }

        [HttpPut("employee")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeToReturnDto>> UpdateEmployeeAsync(Employee employee)
        {
            var emp = await _empService.UpdateEmployeeAsync(employee);
            return _mapper.Map<Employee, EmployeeToReturnDto>(emp);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<bool> DeleteEmployeeAsync(Employee employee)
        {
            return await _empService.DeleteEmployeeAsync(employee);
        }

        [HttpGet("skills")]
        public async Task<ActionResult<Pagination<HRSkillClaimsDto>>> GetEmpSkilledForACategory([FromQuery] HRSkillClaimsParam hParam)
        {
            if (hParam.PageIndex == 0)
                return BadRequest(new ApiResponse(400, " Pagination not defined"));

            var skillRepo = _unitOfWork.Repository<HRSkillClaim>();
            var totalItems = await skillRepo.CountWithSpecAsync(new HRSkillClaimsWithFiltersForCountSpec(hParam));

            var listClaims = await skillRepo.GetEntityListWithSpec(new HRSkillClaimsSpecs(hParam));
            if (listClaims == null)
                return NotFound(new ApiResponse(400, "No employees found with requested skills"));

            var data = _mapper.Map<IReadOnlyList<HRSkillClaim>, IReadOnlyList<HRSkillClaimsDto>>(listClaims);

            return Ok(new Pagination<HRSkillClaimsDto>(hParam.PageIndex, hParam.PageSize, totalItems, data));
        }

        [HttpPost("registerSkills")]
        public async Task<ActionResult<HRSkillClaimsDto>> RegisterHREmployeeSkills(HRSkillClaim hrskills)
        {
            var sk = await _unitOfWork.Repository<HRSkillClaim>().AddAsync(hrskills);
            if (sk == null)
                return NotFound(new ApiResponse(400, "failed to register the employee skill"));

            return Ok(_mapper.Map<HRSkillClaim, HRSkillClaimsDto>(sk));
        }

        [HttpPut("skills")]
        public async Task<ActionResult<HRSkillClaimsDto>> UpdateEmployeeSkills(HRSkillClaim hrskills)
        {
            var sk = await _unitOfWork.Repository<HRSkillClaim>().UpdateAsync(hrskills);
            if (sk == null)
                return NotFound(new ApiResponse(400, "failed to update the employee skill"));

            return Ok(_mapper.Map<HRSkillClaim, HRSkillClaimsDto>(sk));
        }

        [HttpDelete("employeeskills")]
        public async Task<int> DeleteHREmployeeSkills(HRSkillClaim hrskills)
        {
            return await _unitOfWork.Repository<HRSkillClaim>().DeleteAsync(hrskills);
        }

    }
}