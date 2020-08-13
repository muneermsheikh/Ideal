using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Masters;
using Core.Interfaces;

// this controller is for Users with Authority = AddMasterValues;
// another contorller CategoriesController is used by loggedin users, who are clients or candidates, and
// who have no authority to edit master values
namespace Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Category> CategoryByIdAsync(int Id)
        {
            return await _unitOfWork.Repository<Category>().GetByIdAsync(Id);
        }

        public async Task<IReadOnlyList<Category>> CategoryListAsync()
        {
            return await _unitOfWork.Repository<Category>().ListAllAsync();
        }

        public async Task<Category> CreateCategoryAsync(string name, int indTypeId, int skillLevelId)
        {
            var cat = new Category(name, indTypeId, skillLevelId);
            return await _unitOfWork.Repository<Category>().AddAsync(cat);
            //var result = await _unitOfWork.Complete();
        }

        public async Task<Category> DeleteCategoryByIdAsync(int Id)
        {
            var cat = await _unitOfWork.Repository<Category>().GetByIdAsync(Id);
            if (cat== null) return null;
            
            _unitOfWork.Repository<Category>().Delete(cat);
            var result = await _unitOfWork.Complete();
            if (result == 0) return null;
            return cat;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            // var cat = await _unitOfWork.Repository<Category>().GetByIdAsync(Id);
            _unitOfWork.Repository<Category>().Update(category);
            var result = await _unitOfWork.Complete();
            if (result == 0) return null;
            return category;
        }

    // industry types

        public async Task<IReadOnlyList<IndustryType>> GetIndustryTypes()
        {
            return await _unitOfWork.Repository<IndustryType>().ListAllAsync();
        }

        public async Task<IndustryType> CreateIndustryType(string name)
        {
            var indType = new IndustryType(name);
            return await _unitOfWork.Repository<IndustryType>().AddAsync(indType);
        }

        public async Task<IndustryType> UpdateIndustryType(IndustryType indType)
        {
            return await _unitOfWork.Repository<IndustryType>().UpdateAsync(indType);
        }

        public async Task<int> DeleteIndustryType(IndustryType indType)
        {
            return await _unitOfWork.Repository<IndustryType>().DeleteAsync(indType);
        }
        public async Task<IndustryType> DeleteIndustryTypeById(int Id)
        {
            var ind = await _unitOfWork.Repository<IndustryType>().GetByIdAsync(Id);
            if (ind== null) return null;
            
            _unitOfWork.Repository<IndustryType>().Delete(ind);
            var result = await _unitOfWork.Complete();
            if (result == 0) return null;
            return ind;
        }


// skill Level
        public async Task<SkillLevel> CreateSkillLevel(string name)
        {
            var skillLvl = new SkillLevel(name);
            return await _unitOfWork.Repository<SkillLevel>().AddAsync(skillLvl);
        }

        public async Task<SkillLevel> UpdateSkillLevel(SkillLevel skLevel)
        {
            return await _unitOfWork.Repository<SkillLevel>().UpdateAsync(skLevel);
        }

        public async Task<int> DeleteSkillLevel(SkillLevel skLevel)
        {
            return await _unitOfWork.Repository<SkillLevel>().DeleteAsync(skLevel);
        }
        
        public async Task<IReadOnlyList<SkillLevel>> GetSkillLevels()
        {
            return await _unitOfWork.Repository<SkillLevel>().ListAllAsync();
        }


        public async Task<SkillLevel> DeleteSkillLevelById(int Id)
        {
            var skl = await _unitOfWork.Repository<SkillLevel>().GetByIdAsync(Id);
            if (skl== null) return null;
            
            _unitOfWork.Repository<SkillLevel>().Delete(skl);
            var result = await _unitOfWork.Complete();
            if (result == 0) return null;
            return skl;
        }
    }
}
