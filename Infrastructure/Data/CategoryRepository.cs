using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Masters;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ATSContext _context;
        public CategoryRepository(ATSContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Category>> GetCategoriesAsync()
        {
            return await _context.Categories
                .Include(x => x.IndustryType)
                .Include(x => x.SkillLevel)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                .Include(x => x.IndustryType)
                .Include(x => x.SkillLevel)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<IndustryType>> GetIndustryTypesAsync()
        {
            return await _context.IndustryTypes.ToListAsync();
        }

        public async Task<IReadOnlyList<SkillLevel>> GetSkillLevelsAsync()
        {
            return await _context.SkillLevels.ToListAsync();
        }
    }
}