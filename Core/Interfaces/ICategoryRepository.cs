using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Masters;

namespace Core.Interfaces
{
    public interface ICategoryRepository
    {
         Task<Category>GetCategoryByIdAsync(int id);
         Task<IReadOnlyList<Category>>GetCategoriesAsync();
         Task<IReadOnlyList<IndustryType>>GetIndustryTypesAsync();
         Task<IReadOnlyList<SkillLevel>>GetSkillLevelsAsync();
    }
}