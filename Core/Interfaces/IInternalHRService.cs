using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.HR;
using Core.Entities.Masters;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IInternalHRService
    {
        Task<HRSkillClaim> AddEmployeeHRSkill(HRSkillClaim hrSkillClaims);
        Task<HRSkillClaim> UpdateEmployeeHRSkill(HRSkillClaim hrSkillClaims);
        Task<int> DeleteEmployeeHRSkill(HRSkillClaim hrSkillClaims);
        
    }
}