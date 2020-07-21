using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.HR;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IHRService
    {
        Task<Assessment>GetCandidateAssessmentSheet(int CandidateId, int EnquiryItemId, int UserId, string userDisplayName);
        Task<Assessment>EditAssessment(Assessment assessment);
        Task<IReadOnlyList<Assessment>> GetAssessmentList(int EnquiryItemId);
    }
}