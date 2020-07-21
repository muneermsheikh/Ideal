using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities.Masters;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EmployeesController : BaseApiController
    {
        private readonly IEmployeeService _empService;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Employee> _repoEmp;

        public EmployeesController(IEmployeeService empService, IMapper mapper,
        IGenericRepository<Employee> repoEmp)
        {
            _repoEmp = repoEmp;
            _mapper = mapper;
            _empService = empService;
        }

    [HttpGet]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeById(int EmployeeId)
    {
        var emp = await _empService.GetEmployeeByIdAsync(EmployeeId);
        if (emp == null) return NotFound(new ApiResponse(404));
        var empDto = _mapper.Map<Employee, EmployeeDto>(emp);
        return empDto;
    }

    [HttpGet]
    public async Task<ActionResult<Pagination<EmployeeDto>>> GetEmployeeList(EmployeeParam empParam)
    {
        var spec = new EmployeeSpecs(empParam);
        var specCount = new EmployeeSpecWithCount(empParam);
        var totalItems = await _repoEmp.CountWithSpecAsync(specCount);

        var emps = await _repoEmp.GetEntityListWithSpec(spec);
        if (emps == null) return NotFound(new ApiResponse(404, "No employees found matching the criteria"));

        var data = _mapper
            .Map<IReadOnlyList<Employee>, IReadOnlyList<EmployeeDto>>(emps);
        return Ok(new Pagination<EmployeeDto>
                (empParam.PageIndex, empParam.PageSize, totalItems, data));
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateNewEmployeeAsync(EmployeeToAddDto emp)
    {
        var empToAdd = await _empService.CreateNewEmployeeAsync(emp.Gender, emp.FirstName,
            emp.SecondName, emp.FamilyName, emp.KnownAs, emp.Address1, emp.Address2,
            emp.City, emp.PIN, emp.District, emp.State, "India", emp.Mobile, emp.Email,
            emp.AadharNo, emp.PassportNo, emp.Designation, emp.DateOfBirth, emp.DateOfBirth);
        if (empToAdd == null) return BadRequest(402);

        var empAdded = _mapper.Map<Employee, EmployeeDto>(empToAdd);
        return empAdded;
    }

}
}