using System;
using System.Linq.Expressions;
using Core.Entities.Masters;

namespace Core.Specifications
{
    public class SkillLevelSpec : BaseSpecification<SkillLevel>
    {
        //No separate SkillLevelParam necessary, as IndustryTypeParam can be used in its place
        public SkillLevelSpec(IndTypeSpecParams param) 
            : base( x => (
                (string.IsNullOrEmpty(param.Search) || 
                    x.Name.ToLower().Contains(param.Search)) &&
                (!param.Id.HasValue || x.Id == param.Id)))
        {
            AddOrderBy(x => x.Name);
            ApplyPaging(param.PageSize * (param.PageIndex-1), param.PageSize);
        }
    }
}