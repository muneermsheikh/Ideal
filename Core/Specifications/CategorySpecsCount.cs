using Core.Entities.Masters;

namespace Core.Specifications
{
    public class CategorySpecsCount: BaseSpecification<Category>
    {
        public CategorySpecsCount(CategorySpecsParams catParams)
            : base(x => 
                (string.IsNullOrEmpty(catParams.Search) || x.Name.ToLower().Contains(catParams.Search)) &&
                (!catParams.IndustryTypeId.HasValue || x.IndustryTypeId == catParams.IndustryTypeId) &&
                (!catParams.SkillLevelId.HasValue || x.SkillLevelId == catParams.SkillLevelId)
            )
        {
        }
        
        
    }
}