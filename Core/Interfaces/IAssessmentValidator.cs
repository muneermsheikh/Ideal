using Core.Entities.HR;

namespace Core.Interfaces
{
    public interface IAssessmentValidator
    {
        string ValidateAssessment(Assessment assessment);
    }
}