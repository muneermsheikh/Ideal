using System.Threading.Tasks;
using Core.Entities.EnquiryAggregate;
using Core.Entities.HR;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;

namespace API.Validations
{
    public class CVEvaluationValidations
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskService _taskService;
        public CVEvaluationValidations(IUnitOfWork unitOfWork, ITaskService taskService)
        {
            _taskService = taskService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> OkayToSubmitCVByHRExec(int EnquiryItemId, 
            int CandidateId, int UserId)
        {
            // check if the category item is approved + requires evaluation by HR Supervisor
            var spec = new EnquiryItemsSpecs(EnquiryItemId, enumItemReviewStatus.Accepted, true);
            var enqItem = await _unitOfWork.Repository<EnquiryItem>().GetEntityWithSpec(spec);
            if (enqItem == null) return false;
            
            // check if the User is tasked with the HR task
            var t = await _taskService.GetTaskListAsync(EnquiryItemId, enumTaskType.HRExecutiveAssignment,
                enumTaskStatus.NotStarted, true, UserId);
            
            if ( t == null) return false;       // not tasked

            // check if the candidate is assed for the same requirement earlier
            var evalSpec = new CVEvaluationSpecs(EnquiryItemId, CandidateId);
            var assessed = await _unitOfWork.Repository<CVEvaluation>().GetEntityWithSpec(evalSpec);
            if (assessed != null) return false;

            return true;
        }
    }
}