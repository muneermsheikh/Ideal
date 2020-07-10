using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Masters;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _repo;
        public CategoryService(IGenericRepository<Category> repo)
        {
            _repo = repo;
        }

        public Task<Category> CreateCategoryAsync(string categoryName, int industryTypeId, int skillLevelId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteCategoryById(int Id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IReadOnlyList<Category>> GetCategoriesAsync()
        {
            return await _repo.ListAllAsync();
        }

        public async Task<Category> GetCategoryById(int Id)
        {
            return await _repo.GetByIdAsync(Id);

            // var spec = new CategoryWithIndTypeAndSkillLevelSpec(id);
            
            // var cat = await _catRepo.GetEntityWithSpec(spec);
            // if (cat == null) return NotFound(new ApiResponse(404));

            // return _mapper.Map<Category, CategoryToReturnDto>(cat);

        }
    }
}