using Core.Entities.Masters;

namespace Core.Specifications
{
    public class CategoriesWithFiltersForCountSpec: BaseSpecification<Category>
    {
        public CategoriesWithFiltersForCountSpec(CategorySpecParams catParams)
        : base( x => (
                (string.IsNullOrEmpty(catParams.Search) || 
                    x.Name.ToLower().Contains(catParams.Search)) &&
                (!catParams.IndustryTypeId.HasValue || 
                    x.IndustryTypeId == catParams.IndustryTypeId) &&
                (!catParams.SkillLevelId.HasValue || 
                    x.SkillLevelId == catParams.SkillLevelId)
            ))
        {

        }
    }
}