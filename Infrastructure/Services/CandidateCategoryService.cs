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

        public async Task<CandidateCategory> AddCandidateCategory(int candidateId, int categoryId)
        {
            return await _candCatRepo.AddAsync(
                new CandidateCategory(candidateId, categoryId));
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
                .Where(p => CategoryIntIds.Contains(p.CatId))
                .Select(x => x.CandId).ToListAsync();
        
            if (candidateIds == null) return null;

            return await _context.Candidates.Where(x => candidateIds.Contains(x.Id)).ToListAsync();
        }

        public async Task<CandidateCategory> UpdateCandidateCategory(CandidateCategory candidateCategory)
        {
            return await _candCatRepo.UpdateAsync(candidateCategory);
        }

        public async Task<List<Category>> GetCandidateCategories(int candidateId)
        {
            var cats = await _context.CandidateCategories
                .Where(x => x.CandId == candidateId).Select(x => x.CatId).ToListAsync();
            if (cats==null || cats.Count == 0) return null;

            var candCats = await _context.Categories .Where(x => cats.Contains(x.Id))
                .Include(x => x.IndustryType).Include(x=>x.SkillLevel).ToListAsync();
            if (candCats==null || candCats.Count==0) return null;
            return candCats;
        }

        public async Task<List<CandidateCategory>> GetCandidateCategoryType(int candidateId)
        {
            var cats = await _context.CandidateCategories
                .Where(x => x.CandId == candidateId).ToListAsync();
            return cats;
        }
    }
}