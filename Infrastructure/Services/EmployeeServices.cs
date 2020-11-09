using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.HR;
using Core.Entities.Identity;
using Core.Entities.Masters;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class EmployeeServices : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ATSContext _context;
        public EmployeeServices(IUnitOfWork unitOfWork, ATSContext context)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }


        public async Task<Employee> CreateNewEmployeeAsync(Employee emp)
        {
  
            var empSaved = await _unitOfWork.Repository<Employee>().AddAsync(emp);
            // above saves related entities address ad skills also

            /*
            if (empSaved !=null) {
                int empId = empSaved.Id;
                
                var newAdds = new List<EmployeeAddress>();
                var adds = emp.Addresses;
                foreach (var addr in adds) {
                    if (addr != null) {
                        addr.EmployeeId=empId;
                        newAdds.Add(addr);
                    }
                }
                if (newAdds !=null && newAdds.Count > 0) {
                    var addsSaved = await _unitOfWork.Repository<EmployeeAddress>().AddListAsync(newAdds);
                }

                var newSkills = new List<Skill>();
                var sk = emp.Skills;

                if (sk != null) {
                foreach (var s in sk) {
                    if (s != null) {
                        s.EmployeeId = empId;
                        newSkills.Add(s);
                    }
                }
                }

                if (newSkills != null && newSkills.Count > 0) {
                    var sks = await _unitOfWork.Repository<Skill>().AddListAsync(newSkills);
                }
            }
            */ 
            
            return empSaved;
        }

        public async Task<Employee> CreateNewEmployeeAsync(string gendr, string firstNm,
            string secondNm, string familyNm, string knownAs, 
        /*  string add1, string add2,
            string city, string pin, string district, string state, string country, 
        */
            string mobile,
            string email, string aadharNumber, string passportNo, string Designation,
            DateTime DateOfBirth, DateTime DateOfJoining)
        {
            var adds = new List<EmployeeAddress>();
        /*    var add = new EmployeeAddress("R", add1, add2, city, pin, district, state);
            adds.Add(add);
        */
            var emp = new Employee(firstNm, secondNm, familyNm, knownAs, gendr, DateOfBirth,
                passportNo, aadharNumber, mobile, email, adds, Designation, DateOfJoining);

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
        public string GetEmployeeName(int employeeId)
        {
            return _context.Employees.Where(x=>x.Id==employeeId).Select(x=>x.FullName).FirstOrDefault();
        }

    }
}