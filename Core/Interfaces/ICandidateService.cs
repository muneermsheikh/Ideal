using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.HR;
using Core.Entities.Masters;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface ICandidateService
    {
         Task<Candidate> RegisterCandidate(Candidate candidate);
         Task<IReadOnlyList<Candidate>> RegisterCandidates(IReadOnlyList<Candidate> candidate);
         Task<Candidate> UpdateCandidate (Candidate candidateToUpdateDto);
         Task<int> DeleteCandidate(int id);
         Task<Candidate> GetCandidateBySpec(CandidateParams candidateParams);
         Task<Candidate> GetCandidateById(int candidateId);
         Task<Candidate> GetCandidateByApplicationNo(int ApplicationNo);
         Task<IReadOnlyList<Candidate>> GetCandidatesBySpecs(CandidateParams candidateParams);
         string GetCandidateName (int candidateId);
         
         Task<Candidate> CandidateAppNoOrPPNoOrAadharNoOrEmailExist(int appno, string ppno, string aadharno, string email);
         Task<Candidate> PPNumberExists(string ppnumber);
         Task<Candidate> AadharNumberExists(string aadharNo);

    //sources
        Task<IReadOnlyList<Source>> GetSources();

        Task<List<Category>> GetCandCatsWithProf();
        
    }
}