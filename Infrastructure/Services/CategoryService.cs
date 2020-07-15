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
            _unitOfWork.Repository<Category>().Add(cat);
            var result = await _unitOfWork.Complete();
            return (result > 0)  ? cat : null;
        }

        public async Task<Category> DeleteCategoryByIdAsync(int Id)
        {
            var cat = await _unitOfWork.Repository<Category>().GetByIdAsync(Id);
            _unitOfWork.Repository<Category>().Delete(cat);
            var result = await _unitOfWork.Complete();
            if (result == 0) return null;
            return cat;
        }

        public async Task<Category> UpdateCategoryByIdAsync(int Id)
        {
            var cat = await _unitOfWork.Repository<Category>().GetByIdAsync(Id);
            _unitOfWork.Repository<Category>().Update(cat);
            var result = await _unitOfWork.Complete();
            if (result == 0) return null;
            return cat;
        }
    }
}
