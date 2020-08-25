using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.HR;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IAssessmentService
    {
        Task<Assessment> AddAssessment(Assessment assessment);
        Task<IReadOnlyList<Assessment>> GetAssessmentBySpec(AssessmentParam assessmentParam);
        Task<int> DeleteAssessment(Assessment assessment);

        
    }
}