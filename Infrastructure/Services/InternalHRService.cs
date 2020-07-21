using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.HR;
using Core.Entities.Masters;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class InternalHRService : IInternalHRService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ATSContext _context;

        public InternalHRService()
        {
        }

        public InternalHRService(IUnitOfWork unitOfWork, ATSContext context)
        
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<HRSkillClaim> AddEmployeeHRSkill(HRSkillClaim hrSkillClaims)
        {
            return await _unitOfWork.Repository<HRSkillClaim>().AddAsync(hrSkillClaims);
        }

     
        public async Task<int> DeleteEmployeeHRSkill(HRSkillClaim hrSkillClaims)
        {
            return await _unitOfWork.Repository<HRSkillClaim>().DeleteAsync(hrSkillClaims);
        }

        public async Task<HRSkillClaim> UpdateEmployeeHRSkill(HRSkillClaim hrSkillClaims)
        {
            return await _unitOfWork.Repository<HRSkillClaim>().UpdateAsync(hrSkillClaims);
        }
      
    }
}