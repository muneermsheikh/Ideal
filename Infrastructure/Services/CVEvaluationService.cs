using System;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Entities.HR;
using Core.Entities.Masters;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class CVEvaluationService : ICVEvaluationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskService _taskService;
        public CVEvaluationService(IUnitOfWork unitOfWork, ITaskService taskService)
        {
            _taskService = taskService;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> DeleteCVForEvaluation_ByHRExec(CVEvaluation eval)
        {
            if (eval.ReviewedByHRSup == true || eval.ReviewedByHRM == true) return 0;
            return await _unitOfWork.Repository<CVEvaluation>().DeleteAsync(eval);
        }

        public async Task<CVEvaluation> CVForEvaluation_ByHRExec(int CandidateId, int EnquiryItemId, int UserId)
        {
            var b = IsOkToSubmitCVByHRExec(EnquiryItemId, CandidateId, UserId).Result;
            if (b==true) return null;
            
            // get candidatename
            var cand = await GetCandidateEntity(CandidateId);
            var item = await GetEnquiryItem(EnquiryItemId);
            
            // first step in the process of cv evaluation (followd by evaluation by supervisor
            // and HR manager), therefore need to create a record
            var eval = new CVEvaluation(EnquiryItemId, cand.Id, cand.FullName, 
                cand.ApplicationNo, UserId,item.AssessingSupId,item.AssessingHRMId);
            var evalEntity = await _unitOfWork.Repository<CVEvaluation>().AddAsync(eval);            
            return evalEntity;
        }

        private async Task<EnquiryItem> GetEnquiryItem(int enquiryItemId)
        {
            var spec = new EnquiryItemsSpecs(enquiryItemId);
            return await _unitOfWork.Repository<EnquiryItem>().GetByIdAsync(enquiryItemId);
        }

        private async Task<Candidate> GetCandidateEntity(int candidateId)
        {
            return await _unitOfWork.Repository<Candidate>().GetByIdAsync(candidateId);
        }


        private async Task<bool> IsOkToSubmitCVByHRExec(int EnquiryItemId,
            int CandidateId, int UserId)
        {
            // check if the category item is approved + requires evaluation by HR Supervisor
            var spec = new EnquiryItemsSpecs(EnquiryItemId, enumItemReviewStatus.Accepted, true);
            var enqItem = await _unitOfWork.Repository<EnquiryItem>().GetEntityWithSpec(spec);
            if (enqItem == null) return false;

            // check if the User is tasked with the HR task
            var t = await _taskService.GetTaskListAsync(EnquiryItemId, enumTaskType.HRExecutiveAssignment,
                enumTaskStatus.NotStarted, true, UserId);

            if (t == null) return false;       // not tasked

            // check if the candidate is assed for the same requirement earlier
            var evalSpec = new CVEvaluationSpecs(EnquiryItemId, CandidateId);
            var assessed = await _unitOfWork.Repository<CVEvaluation>().GetEntityWithSpec(evalSpec);
            if (assessed != null) return false;

            return true;
        }

        public async Task<CVEvaluation> CVForEvaluation_ByHRSup(CVEvaluation cvEval)
        {
            if (cvEval.SubmittedByHRExecOn == null ||
                cvEval.ReviewedByHRSupOn == null || 
                cvEval.ReviewedByHRSupOn < cvEval.SubmittedByHRExecOn ||
                cvEval.HRSupReviewResult == null) return null;
            
            if (cvEval.ReviewedByHRSup != true) cvEval.ReviewedByHRSup=true;

            await _unitOfWork.Repository<CVEvaluation>().UpdateAsync(cvEval);
            
        // get deails required for creating a task in the name of HRSupevisor    
            var empHRExec = await _unitOfWork.Repository<Employee>().GetByIdAsync(cvEval.HRExecutiveId);
            var empHRSup= await _unitOfWork.Repository<Employee>().GetByIdAsync((int)cvEval.HRSupervisorId);
            var enq = await _unitOfWork.Repository<Enquiry>().GetByIdAsync(cvEval.EnquiryItem.EnquiryId);

        // create task for HR Supervisor
            var taskForHRSupervisor = new ToDo((int)cvEval.HRSupervisorId, (int)cvEval.HRManagerId, 
                DateTime.Now, DateTime.Now.AddDays(1), 
                empHRExec.KnownAs + " has forwarded Application " +
                cvEval.Candidate.ApplicationNo + "-" + cvEval.Candidate.FullName + 
                "for your review for requirement ref " + enq.EnquiryNo + "-" + 
                cvEval.EnquiryItem.SrNo + "-" + cvEval.EnquiryItem.CategoryName, 
                enumTaskType.HRSupervisorAssignment, cvEval.EnquiryItem.EnquiryId,
                cvEval.EnquiryItem.Id);
            await _taskService.CreateTaskAsync(taskForHRSupervisor);

        // append a record to the task of HR Executive 
            // 1 - get ToDo.Id
            var taskHRExec = _taskService.GetTaskEnquiryitemIdAssignedToIdTaskTypeAsync(
                cvEval.EnquiryItemId, cvEval.HRExecutiveId, enumTaskType.HRExecutiveAssignment);
            if (taskHRExec == null) return null;
            var taskItem = new TaskItem(taskHRExec.Id, DateTime.Now, 1,
                "Application " + cvEval.Candidate.ApplicationNo + "submitted for HR Supervisor review",
                true, null);
            await _taskService.AppendTaskItemAsync(taskHRExec.Id,taskItem);
            return cvEval;
        }

        public async Task<CVEvaluation> CVForEvaluation_ByHRM(CVEvaluation cvEval)
        {
            if (cvEval.ReviewedByHRSupOn == null ||
                cvEval.ReviewedByHRMOn < cvEval.ReviewedByHRSupOn ||
                cvEval.HRMReviewResult == null) return null;
            if (cvEval.ReviewedByHRM != true) cvEval.ReviewedByHRM=true;

            await _unitOfWork.Repository<CVEvaluation>().UpdateAsync(cvEval);
            
            var empHRM = await _unitOfWork.Repository<Employee>().GetByIdAsync((int)cvEval.HRManagerId);
            var empHRSup= await _unitOfWork.Repository<Employee>().GetByIdAsync((int)cvEval.HRSupervisorId);
            var enq = await _unitOfWork.Repository<Enquiry>().GetByIdAsync(cvEval.EnquiryItem.EnquiryId);

        // create task for Doc Controller - Admin
            var taskForDocControllerAdmin = new ToDo((int)cvEval.HRSupervisorId, (int)cvEval.HRManagerId, 
                DateTime.Now, DateTime.Now.AddDays(1), 
                empHRSup.KnownAs + " has forwarded Application " +
                cvEval.Candidate.ApplicationNo + "-" + cvEval.Candidate.FullName + 
                "for your review for requirement ref " + enq.EnquiryNo + "-" + 
                cvEval.EnquiryItem.SrNo + "-" + cvEval.EnquiryItem.CategoryName, 
                enumTaskType.HRSupervisorAssignment, cvEval.EnquiryItem.EnquiryId,
                cvEval.EnquiryItem.Id);
            await _taskService.CreateTaskAsync(taskForDocControllerAdmin);

        // append a record to the task of HR Executive
            // 1 - get ToDo.Id
            var taskHRExec = _taskService.GetTaskEnquiryitemIdAssignedToIdTaskTypeAsync(
                cvEval.EnquiryItemId, cvEval.HRExecutiveId, enumTaskType.HRExecutiveAssignment);
            if (taskHRExec == null) return null;
            var taskItem = new TaskItem(taskHRExec.Id, DateTime.Now, 1,
                "Application " + cvEval.Candidate.ApplicationNo + "submitted for HR Supervisor review",
                true, null);
            await _taskService.AppendTaskItemAsync(taskHRExec.Id,taskItem);
            return cvEval;

        }

    }
}