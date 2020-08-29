using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.HR;
using Core.Entities.Masters;

namespace Core.Interfaces
{
    public interface ICandidateCategoryService
    {
        Task<CandidateCategory> AddCandidateCategory(int CandidateId, int CategoryId);
        Task<IReadOnlyList<CandidateCategory>> AddCandidateCategories(IReadOnlyList<CandidateCategory> candidateCategoryList);
        Task<CandidateCategory> UpdateCandidateCategory(CandidateCategory candidateCategory);
        Task<int> UpdateCandidateCategories( List<CandidateCategory> candcategories);
        Task<int> DeleteCandidatecategory(CandidateCategory candidateCategory);
        Task<List<Category>> GetCandidateCategories(int candidateId);
        Task<List<CandidateCategory>> GetCandidateCategoryType(int candidateId);

        Task<IReadOnlyList<Candidate>> GetCandidatesWithMatchingCategories(IReadOnlyList<int> CategoryIds);
        
    }
}