using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Entities.HR;
using Core.Entities.Masters;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class CandidateCategoryService : ICandidateCategoryService
    {
        private readonly IGenericRepository<CandidateCategory> _candCatRepo;
        private readonly ATSContext _context;
        public CandidateCategoryService(IGenericRepository<CandidateCategory> candCatRepo, 
            ATSContext context)
        {
            _context = context;
            _candCatRepo = candCatRepo;
        }

        public async Task<CandidateCategory> AddCandidateCategory(int candidateId, int categoryId, string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                categoryName = await _context.Categories.Where(x => x.Id == categoryId)
                    .Select(x => x.Name).SingleOrDefaultAsync();
            }
            return await _candCatRepo.AddAsync(
                new CandidateCategory(candidateId, categoryId, categoryName));
        }


        public async Task<IReadOnlyList<CandidateCategory>> AddCandidateCategories(IReadOnlyList<CandidateCategory> candidateCategoryList)
        {
            return await _candCatRepo.AddListAsync(candidateCategoryList);
        }

        public async Task<int> DeleteCandidatecategory(CandidateCategory candidateCategory)
        {
            return await _candCatRepo.DeleteAsync(candidateCategory);
        }


        public async Task<IReadOnlyList<Candidate>> GetCandidatesWithMatchingCategories(
            IReadOnlyList<int> CategoryIntIds)
        {
            var candidateIds = await _context.CandidateCategories
                .Where(p => CategoryIntIds.Contains(p.CategoryId))
                .Select(x => x.CandidateId).ToListAsync();
        
            if (candidateIds == null) return null;

            return await _context.Candidates.Where(x => candidateIds.Contains(x.Id)).ToListAsync();
        }

        public async Task<CandidateCategory> UpdateCandidateCategory(CandidateCategory candidateCategory)
        {
            return await _candCatRepo.UpdateAsync(candidateCategory);
        }

        public async Task<int> UpdateCandidateCategories( List<CandidateCategory> candcategories)
        {
            return await _candCatRepo.UpdateListAsync(candcategories);
        }
        public async Task<List<Category>> GetCandidateCategories(int candidateId)
        {
            var cats = await _context.CandidateCategories
                .Where(x => x.CandidateId == candidateId).Select(x => x.CategoryId).ToListAsync();
            if (cats==null || cats.Count == 0) return null;

            var candCats = await _context.Categories .Where(x => cats.Contains(x.Id))
                .Include(x => x.IndustryType).Include(x=>x.SkillLevel).ToListAsync();
            if (candCats==null || candCats.Count==0) return null;
            return candCats;
        }

        public async Task<List<CandidateCategory>> GetCandidateCategoryType(int candidateId)
        {
            var cats = await _context.CandidateCategories
                .Where(x => x.CandidateId == candidateId).ToListAsync();
            return cats;
        }

        public async Task<List<Category>> GetCandididateCatsWithProf()
        {
            return await _context.Categories.OrderBy(x => x.Name).ToListAsync();
        }
    }
}