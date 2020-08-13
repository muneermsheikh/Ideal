using System;
using System.Linq.Expressions;
using Core.Entities.Masters;

namespace Core.Specifications
{
    public class CategorySpecs : BaseSpecification<Category>
    {
        public CategorySpecs(CategorySpecsParams catParams)
            : base(x => 
                (string.IsNullOrEmpty(catParams.Search) || x.Name.ToLower().Contains(catParams.Search)) &&
                (!catParams.IndustryTypeId.HasValue || x.IndustryTypeId == catParams.IndustryTypeId) &&
                (!catParams.SkillLevelId.HasValue || x.SkillLevelId == catParams.SkillLevelId)
            )
        {
            AddInclude(x => x.IndustryType);
            AddInclude(x => x.SkillLevel);
            AddOrderBy(x => x.Name);

            ApplyPaging(catParams.PageSize * (catParams.PageIndex-1), catParams.PageSize);

            if (!string.IsNullOrEmpty(catParams.Sort))
            {
                switch (catParams.Sort)
                {
                    case "CategoryNameAsc":
                        AddOrderBy(x => x.Name);
                        break;
                    case "CategoryNameDesc":
                        AddOrderByDescending(x => x.Name);
                        break;
                    case "IndustryTypeAsc":
                        AddOrderBy(x => x.IndustryType.Name);
                        break;
                    case "IndustryTypeDesc":
                        AddOrderByDescending(x => x.IndustryType.Name);
                        break;
                    case "SkillLevelAsc":
                        AddOrderBy(x => x.SkillLevel.Name);
                        break;
                    case "SkillLevelDesc":
                        AddOrderBy(x => x.SkillLevel.Name);
                        break;
                    default:
                        AddOrderBy(x => x.Name);
                        break;
                }
            }
        }

        public CategorySpecs(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.IndustryType);
            AddInclude(x => x.SkillLevel);
        }
    }
}