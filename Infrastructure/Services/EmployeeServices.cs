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
using Microsoft.EntityFrameworkCore;

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

        public async Task<Employee> UpdateEmployeeAsync( Employee emp)
        {
            //delete all related entities skills and addresses
            var EmpAddresses = await _context.EmployeeAddresses.Where(x => x.EmployeeId == emp.Id).ToListAsync();
            if (EmpAddresses != null) {
                await _unitOfWork.Repository<EmployeeAddress>().DeleteListAsync(EmpAddresses);
            }

            var EmpSkills = await _context.Skills.Where(x => x.EmployeeId == emp.Id).ToListAsync();
            if (EmpSkills != null) {
                await _unitOfWork.Repository<Skill>().DeleteListAsync(EmpSkills);
            }

            await _unitOfWork.Repository<Employee>().UpdateAsync(emp);

            // finally, update all entities
            await _context.SaveChangesAsync();

            return await _context.Employees.Where(x => x.Id == emp.Id)
                .Include(x => x.Addresses).Include(x => x.Skills).SingleOrDefaultAsync();

        /*
            //flg not working, as only one record in related entities is retained and 
            //others are deleted.
            var existingEmpInDB = await _context.Employees.Where(x => x.Id == emp.Id)
                .Include(x => x.Skills)
                .Include(x => x.Addresses)
                .SingleOrDefaultAsync();
            
            if (existingEmpInDB == null) return null;

            var ExistingSkillsInDB = existingEmpInDB.Skills;
            var ExistingAddressesInDB = existingEmpInDB.Addresses;
            var SkillsInModel = emp.Skills;
            var AddressesInModel = emp.Addresses;

            // in preparation for entities present in DB but not in the models, first
            // identify such entries
            var SkillsMissingInModel = ExistingSkillsInDB.Where(x => !SkillsInModel.Select(b => b.EmployeeId)
                .Contains(x.EmployeeId)).ToList();
            var AddressesMissingInModel = ExistingAddressesInDB.Where(x => !AddressesInModel.Select(b => b.EmployeeId)
                .Contains(x.EmployeeId)).ToList();


            foreach(var SkillToDelete in SkillsMissingInModel)
            {
                ExistingSkillsInDB.Remove(SkillToDelete);
            }

            foreach(var AddressToDelete in AddressesMissingInModel)
            {
                ExistingAddressesInDB.Remove(AddressToDelete);
            }
        
            //update parent
            _context.Entry(existingEmpInDB).CurrentValues.SetValues(emp);
    
            //update model skills whose Id present in existingDB skills
            foreach (var modelSkill in emp.Skills)
            {
                var existingSkillinDB = ExistingSkillsInDB.Where(c => c.Id == modelSkill.Id).SingleOrDefault();
                if (existingSkillinDB != null)
                {
                    _context.Entry(existingSkillinDB).CurrentValues.SetValues(modelSkill);
                } else {
                    // insert skill
                    existingEmpInDB.Skills.Add(modelSkill);
                }
            }

            // work with Addresses children
            //update model skills whose Id present in existing skills
            foreach (var addInModel  in emp.Addresses)
            {
                var existingAddInDB = ExistingAddressesInDB.Where(c => c.Id == addInModel.Id).SingleOrDefault();
                if (existingAddInDB != null)
                {
                    _context.Entry(existingAddInDB).CurrentValues.SetValues(addInModel);
                } else {
                    // insert skill
                    existingEmpInDB.Addresses.Add(addInModel);
                }
            }


            // finally, update all entities
            await _context.SaveChangesAsync();

            return await _context.Employees.Where(x => x.Id == emp.Id)
                .Include(x => x.Addresses).Include(x => x.Skills).SingleOrDefaultAsync();
        */
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


    /* this is repeated above
        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
      //return await _unitOfWork.Repository<Employee>().UpdateAsync(employee);
            var lstSkills = await _context.Skills.Where(x => x.EmployeeId == employee.Id).ToListAsync();

            if (lstSkills != null || lstSkills.Count > 0) {
                foreach (Skill sk in lstSkills)
                {
                    await _unitOfWork.Repository<Skill>().DeleteAsync(sk);
                }
            }

            var lstAdds = await _context.EmployeeAddresses.Where(x => x.EmployeeId == employee.Id).ToListAsync();

            if (lstAdds != null || lstAdds.Count > 0) {
                foreach(EmployeeAddress add in lstAdds)
                {
                    await _unitOfWork.Repository<EmployeeAddress>().DeleteAsync(add);
                }
            }

            await _unitOfWork.Repository<Employee>().UpdateAsync(employee);

            await _context.SaveChangesAsync();
            return _context.Employees.Find(employee.Id);
 
            // db.Entry(parent).State = EntityState.Modified;
            _context.Entry(employee).State = EntityState.Modified;

            // foreach (Child child in parent.Children)
            foreach (Skill sk in employee.Skills)
            {
                // db.Entry(child).State = child.Id == 0 ? EntityState.Added : EntityState.Modified;
                _context.Entry(sk).State = sk.Id == 0 ? EntityState.Added : EntityState.Modified;
            }
            foreach (EmployeeAddress add in employee.Addresses)
            {
                _context.Entry(add).State = add.Id == 0 ? EntityState.Added : EntityState.Modified;
            }

            try
            {
                // await db.SaveChangesAsync();
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return Ok(db.Parents.Find(id));
            return _context.Employees.Find(id);
    
        }
*/
        public string GetEmployeeName(int employeeId)
        {
            return _context.Employees.Where(x=>x.Id==employeeId).Select(x=>x.FullName).FirstOrDefault();
        }

        public async Task<IReadOnlyList<Employee>> GetEmployeeListFlat()
        {
            return await _context.Employees.Where(x => x.IsInEmployment == true).OrderBy(x => x.FirstName).ToListAsync();
        }
    }
}