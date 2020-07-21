using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Dtos;
using Core.Entities.HR;
using Core.Entities.Identity;
using Core.Entities.Masters;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class EmployeeServices : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Employee> CreateNewEmployeeAsync(string gendr, string firstNm,
            string secondNm, string familyNm, string knownAs, string add1, string add2,
            string city, string pin, string district, string state, string country, string mobile,
            string email, string aadharNumber, string passportNo, string Designation,
            DateTimeOffset DateOfBirth, DateTimeOffset DateOfJoining)
        {
            Person person = new Person(firstNm, secondNm, familyNm, knownAs, gendr,
                passportNo, aadharNumber, DateOfBirth);
            var add = new EmployeeAddress("R", add1, add2, city, pin, district, state);
            var emp = new Employee(person, add, Designation, DateOfJoining);

            return await _unitOfWork.Repository<Employee>().AddAsync(emp);
        }


        public async Task<bool> DeleteEmployeeAsync(Employee employee)
        {
            var del = await _unitOfWork.Repository<Employee>().DeleteAsync(employee);
            if (del == 0) return false;
            return true;
        }

        public async Task<Employee> GetEmpDetails(int employeeId)
        {
            return await _unitOfWork.Repository<Employee>().GetByIdAsync(employeeId);
        }

        public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            var spec = new EmployeeSpecs(employeeId);
            return await _unitOfWork.Repository<Employee>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Employee>> GetEmployeeListBySpecAsync(EmployeeParam empParam)
        {
            var spec = new EmployeeSpecs(empParam);
            return await _unitOfWork.Repository<Employee>().GetEntityListWithSpec(spec);
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            return await _unitOfWork.Repository<Employee>().UpdateAsync(employee);
        }

    }
}