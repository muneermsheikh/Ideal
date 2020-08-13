using System;
using System.Linq.Expressions;
using Core.Entities.Masters;

namespace Core.Specifications
{
    public class SkillLevelCount : BaseSpecification<SkillLevel>
    {
        public SkillLevelCount(IndTypeSpecParams param) 
            : base( x => (
                (string.IsNullOrEmpty(param.Search) || 
                    x.Name.ToLower().Contains(param.Search)) &&
                (!param.Id.HasValue || x.Id == param.Id)))
        {
        }
    }
}