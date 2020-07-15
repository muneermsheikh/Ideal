using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Masters;

namespace Core.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> CreateCategoryAsync (string name, int indTypeId, int skillLevelId);
        
        Task<IReadOnlyList<Category>> CategoryListAsync ();

        Task<Category> CategoryByIdAsync(int Id);
        Task<Category> DeleteCategoryByIdAsync(int Id);
        Task<Category> UpdateCategoryByIdAsync(int Id);

    }
}