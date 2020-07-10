using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Masters;

namespace Core.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> CreateCategoryAsync (string categoryName, int industryTypeId, int skillLevelId);
        
        Task<IReadOnlyList<Category>> GetCategoriesAsync ();

        Task<Category> GetCategoryById(int Id);

        Task<bool> DeleteCategoryById(int Id);
    }
}