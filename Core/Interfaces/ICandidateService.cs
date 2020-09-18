using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.HR;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface ICandidateService
    {
         Task<Candidate> RegisterCandidate(Candidate candidate);
         Task<IReadOnlyList<Candidate>> RegisterCandidates(IReadOnlyList<Candidate> candidate);
         Task<Candidate> UpdateCandidate (Candidate candidateToUpdateDto);
         Task<bool> DeleteCandidate(Candidate candidate);
         Task<Candidate> GetCandidateBySpec(CandidateParams candidateParams);
         Task<Candidate> GetCandidateById(int candidateId);
         Task<Candidate> GetCandidateByApplicationNo(int ApplicationNo);
         Task<IReadOnlyList<Candidate>> GetCandidatesBySpecs(CandidateParams candidateParams);
         string GetCandidateName (int candidateId);
    }
}