using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Masters;

namespace Core.Interfaces
{
    public interface ICategoryService
    {
        string GetCategoryNameWithRefFromEnquiryItemId(int enquiryItemId);
        string GetCategoryNameFromCategoryId(int categoryId);
        
        Task<bool> CategoryExists(string nm, int indId, int skId);

        Task<Category> CreateCategoryAsync (string name, int indTypeId, int skillLevelId);
        
        Task<IReadOnlyList<Category>> CategoryListAsync ();

        Task<IReadOnlyList<Category>> CategoriesFromCategoryIds(int[] categoryIds);
        
        Task<Category> CategoryByIdAsync(int Id);
        Task<Category> DeleteCategoryByIdAsync(int Id);
        Task<Category> UpdateCategoryAsync(Category category);

    // IndustryType        
        Task<IReadOnlyList<IndustryType>> GetIndustryTypes();
        Task<IndustryType> CreateIndustryType (string name);
        Task<IndustryType> UpdateIndustryType (IndustryType indType);
        Task<int> DeleteIndustryType (IndustryType indType);
        Task<IndustryType> DeleteIndustryTypeById(int Id);      // need an object to return, to take care of nulls which are returned on error
    
    // skillLevels
        Task<IReadOnlyList<SkillLevel>> GetSkillLevels();
        Task<SkillLevel> CreateSkillLevel(string name);
        Task<SkillLevel> UpdateSkillLevel(SkillLevel skLevel);
        Task<int> DeleteSkillLevel(SkillLevel skLevel);
        Task<SkillLevel> DeleteSkillLevelById (int Id);         //need object as return type, to take care of nulls whicha re returned on error

        

    }
}