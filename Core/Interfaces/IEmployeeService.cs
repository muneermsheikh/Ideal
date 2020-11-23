using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Masters;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IEmployeeService
    {
         Task<Employee> CreateNewEmployeeAsync(string gendr, string firstNm,
            string secondNm, string familyNm, string knownAs, 
      /*      string add1, string add2,
            string city, string pin, string district, string state, string country, 
      */  
            string mobile,
            string email, string aadharNumber, string passportNo, string Designation,
            DateTime DateOfBirth, DateTime DateOfJoining);
         
         Task<Employee> CreateNewEmployeeAsync(Employee emp);

         Task<Employee> UpdateEmployeeAsync(Employee employee);
         Task<bool> DeleteEmployeeAsync(Employee employee);
         Task<IReadOnlyList<Employee>> GetEmployeeListBySpecAsync(EmployeeParam empParam);
         Task<Employee> GetEmployeeByIdAsync(int employeeId);
         Task<Employee> GetEmpDetails(int employeeId);
         Task<IReadOnlyList<Employee>> GetEmployeeListFlat();
         string GetEmployeeName(int employeeId);
    }
}