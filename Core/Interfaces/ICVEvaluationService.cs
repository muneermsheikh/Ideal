using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.HR;
using Core.Enumerations;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface ICVEvaluationService
    {
        Task<CVEvaluation> GetCVEvaluationByIdAsync(int Id);
        Task<CVEvaluation> GetCVEvaluation(int CandidateId, int EnquiryItemId);
        Task<CVEvaluation> CVSubmitToSup(int CandidateId, int EnquiryItemId, int UserId);
        Task<CVEvaluation> CVEvalBySup(int cvEvalId, enumItemReviewStatus status, int supervisorId);
        Task<CVEvaluation> CVEvalByHRM(int cvEvalId, enumItemReviewStatus status, int hrmanagerId);
        Task<int> CVSubmitToSupDelete(CVEvaluation eval);
       
        Task<IReadOnlyList<CVEvaluation>> CVEvaluations(CVEvaluationParam cvEvalParam);
        Task<IReadOnlyList<CVEvaluation>> CVEvaluationsOfEnquiryItemId(int enquiryItemId);

        Task<IReadOnlyList<CVEvaluation>> GetPendingEvaluationOfAUser(int userId);
    }
}