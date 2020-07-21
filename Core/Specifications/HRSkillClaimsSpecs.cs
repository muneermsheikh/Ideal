using System;
using System.Linq.Expressions;
using Core.Entities.HR;

namespace Core.Specifications
{
    public class HRSkillClaimsSpecs : BaseSpecification<HRSkillClaim>
    {
        public HRSkillClaimsSpecs(HRSkillClaimsParam hParam) 
            : base( x => (
                (string.IsNullOrEmpty(hParam.Search) || 
                    x.CategoryName.ToLower().Contains(hParam.Search)) &&
                (!hParam.IndustryTypeId.HasValue || x.IndustryTypeId == hParam.IndustryTypeId) &&
                (!hParam.SkillLevelId.HasValue || x.SkillLevelId == hParam.SkillLevelId)))
        {
        }
    }
}