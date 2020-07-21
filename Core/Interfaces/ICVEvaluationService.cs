using System.Threading.Tasks;
using Core.Entities.HR;

namespace Core.Interfaces
{
    public interface ICVEvaluationService
    {
        Task<CVEvaluation> CVForEvaluation_ByHRExec(
             int CandidateId, int EnquiryItemId, int UserId);
        Task<int> DeleteCVForEvaluation_ByHRExec(CVEvaluation eval);
        Task<CVEvaluation> CVForEvaluation_ByHRSup(CVEvaluation cvEval);
        Task<CVEvaluation> CVForEvaluation_ByHRM(CVEvaluation cvEval);
    }
}