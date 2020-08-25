using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Entities.HR;
using Core.Entities.Masters;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class CVEvaluationService : ICVEvaluationService
    {
        private readonly int constDocControllerAdminId = 5;
        private readonly bool anyOneCanEvaluateCV=true;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskService _taskService;
        private readonly IGenericRepository<CVEvaluation> _repoEval;
        private readonly ATSContext _context;

     
        public CVEvaluationService(IUnitOfWork unitOfWork, ITaskService taskService,       
            IGenericRepository<CVEvaluation> repoEval, ATSContext context)
        {
            _context = context;
            _repoEval = repoEval;
            _taskService = taskService;
            _unitOfWork = unitOfWork;
        }

        public async Task<CVEvaluation> GetCVEvaluationByIdAsync(int Id)
        {
            return await _repoEval.GetByIdAsync(Id);
        }
        public async Task<CVEvaluation> GetCVEvaluation(int CandidateId, int EnquiryItemId)
        {
            var eval = await _repoEval.GetEntityWithSpec(new CVEvaluationSpecs(EnquiryItemId, CandidateId));
            if (eval==null) throw new Exception("No Evaluation on record for the criteria specified");
            return eval;
        }
        
        public async Task<IReadOnlyList<CVEvaluation>> CVEvaluations(CVEvaluationParam cvEvalParam)
        {
            var eval = await _repoEval.GetEntityListWithSpec(new CVEvaluationSpecs(cvEvalParam));
            if (eval==null || eval.Count==0) throw new Exception("No CV evaluation records found matching given criteria");
            return eval;
        }

        public async Task<IReadOnlyList<CVEvaluation>> CVEvaluationsOfEnquiryItemId(int enquiryItemId)
        {
            var spec = new CVEvaluationSpecs(enquiryItemId, "dummmy");
            return await _repoEval.GetEntityListWithSpec(spec);
        }

        // 10 - validate candidateId and EnquiryItemId
        // 20 - validate 
        public async Task<CVEvaluation> CVSubmitToSup(int CandidateId, int EnquiryItemId, int UserId)
        {
            var cand = await _context.Candidates.Where(x => x.Id == CandidateId)
                .Select(x => new {x.ApplicationNo, x.FullName, x.PPNo, x.AadharNo}).FirstOrDefaultAsync();
            if (cand == null) throw new Exception("failed to retrieve candidate with given Id");

            var validated = await ValidateCVSubmissionToSup(EnquiryItemId, CandidateId, UserId);
            if (!validated.Validated) throw new Exception (validated.ErrorMessage); 
            if (validated.NextPhase == "Admin")  throw new Exception("No Supervisor or HRM review registered.  Next" +
                " step would be sending the CV directly to Document Controller for forwwarding to client");

            var empOwner = await _context.Employees.Where(x=>x.Id==UserId)
                .Select(x => new{x.FullName, x.KnownAs}).FirstOrDefaultAsync();
            var enqItem = await _context.EnquiryItems.Where(x => x.Id == EnquiryItemId)
                .Select(x => new {x.CategoryItemId, x.EnquiryId, x.SrNo}).FirstOrDefaultAsync();
            var enq = await _context.Enquiries.Where(x=> x.Id==enqItem.EnquiryId)
                .Select(x => new {x.EnquiryNo, x.EnquiryDate, x.CustomerId}).FirstOrDefaultAsync();

            var eval = new CVEvaluation(EnquiryItemId, CandidateId, cand.FullName,
                cand.ApplicationNo, UserId, validated.supId, validated.HRMId);
            var categoryName = await _context.Categories.Where(x => x.Id == enqItem.CategoryItemId)
                .Select(x=>x.Name).FirstOrDefaultAsync();
            await _unitOfWork.Repository<CVEvaluation>().AddAsync(eval);

            //create task in the name of Sup to evaluate the CV
            var taskForSup = new ToDo (UserId, validated.supId, DateTime.Now, DateTime.Now.AddHours(4), 
                "Application " + cand.ApplicationNo + "-" + cand.FullName + cand.PPNo + 
                " is submitted by " + empOwner.KnownAs + " for your review for the position of " +
                enq.EnquiryNo + "-" + enqItem.SrNo + categoryName, enumTaskType.HRDeptHeadAssignment,
                enqItem.EnquiryId, EnquiryItemId);
            await _taskService.CreateTaskAsync(taskForSup);
            return eval;
        }
        
        public async Task<int> CVSubmitToSupDelete(CVEvaluation eval)
        {
            if (eval.ReviewedByHRSup == true || eval.ReviewedByHRM == true) return 0;
            return await _unitOfWork.Repository<CVEvaluation>().DeleteAsync(eval);
        }
        public async Task<CVEvaluation> CVEvalBySup(int cvEvalId, enumItemReviewStatus status, int supervisorId)
        {
             var cvEval = await _repoEval.GetByIdAsync(cvEvalId);
             if (cvEval==null) throw new Exception("No Evaluation data found matching the id");

            //update the cv evaluation
            cvEval.HRSupervisorId=supervisorId;
            cvEval.ReviewedByHRSup = true;
            cvEval.ReviewedByHRSupOn = DateTime.Now;
            cvEval.HRSupReviewResult=status;

            var cvEvalUpdated = await _unitOfWork.Repository<CVEvaluation>().UpdateAsync(cvEval);

            // update HRSup task
            var taskHRSup = await _taskService.GetTaskEnquiryitemIdAssignedToIdTaskTypeAsync(
                cvEval.EnquiryItemId, (int)cvEval.HRSupervisorId, enumTaskType.HRSupervisorAssignment);
            if (taskHRSup != null) 
            {
                string result = "Application " + cvEval.Candidate.ApplicationNo;
                result += cvEval.HRMReviewResult==enumItemReviewStatus.Accepted ? " approved by HR Manager, " +
                    "and submitted for HRM review" : " rejected by HR Manager with the reason: " + 
                    Enum.GetName(typeof(enumItemReviewStatus), cvEval.HRMReviewResult);

                var taskItem = new TaskItem(taskHRSup.Id, DateTime.Now, cvEval.HRMReviewResult
                    == enumItemReviewStatus.Accepted ? 1: 0, result, true, null);
        
                await _taskService.AppendTaskItemAsync(taskHRSup.Id, taskItem);
                taskHRSup.TaskStatus = enumTaskStatus.Completed;
                await _taskService.UpdateTaskAsync(taskHRSup);
            }

            if (cvEval.HRMReviewResult == enumItemReviewStatus.Accepted)
            {   // create task for Doc Controller - Admin
                if (await CreateTaskForDocControllerToSendCV((int)cvEval.HRManagerId, (int)cvEval.HRSupervisorId, cvEval.CandidateId,
                cvEval.EnquiryItem.EnquiryId, cvEval.EnquiryItem.Id)==false) return null;
            }

            await _repoEval.UpdateAsync(cvEval);
            return cvEval;
        }

        public async Task<CVEvaluation> CVEvalByHRM(int cvEvalId, enumItemReviewStatus status, int hrmanagerId)
        {
            var cvEval = await _repoEval.GetByIdAsync(cvEvalId);
            
            cvEval.ReviewedByHRSup = true;
            cvEval.ReviewedByHRSupOn = DateTime.Now;

            var cvEvalUpdated = await _unitOfWork.Repository<CVEvaluation>().UpdateAsync(cvEval);

            // update HRSup task
            var taskHRSup = await _taskService.GetTaskEnquiryitemIdAssignedToIdTaskTypeAsync(
                cvEval.EnquiryItemId, (int)cvEval.HRSupervisorId, enumTaskType.HRSupervisorAssignment);
            if (taskHRSup != null) 
            {
                string result = "Application " + cvEval.Candidate.ApplicationNo;
                result += cvEval.HRMReviewResult==enumItemReviewStatus.Accepted ? " approved by HR Manager, " +
                    "and submitted for HRM review" : " rejected by HR Manager with the reason: " + 
                    Enum.GetName(typeof(enumItemReviewStatus), cvEval.HRMReviewResult);

                var taskItem = new TaskItem(taskHRSup.Id, DateTime.Now, cvEval.HRMReviewResult
                    == enumItemReviewStatus.Accepted ? 1: 0, result, true, null);
        
                await _taskService.AppendTaskItemAsync(taskHRSup.Id, taskItem);
                taskHRSup.TaskStatus = enumTaskStatus.Completed;
                await _taskService.UpdateTaskAsync(taskHRSup);
            }

            if (cvEval.HRMReviewResult == enumItemReviewStatus.Accepted)
            {   // create task for Doc Controller - Admin
                if (await CreateTaskForDocControllerToSendCV((int)cvEval.HRManagerId, (int)cvEval.HRSupervisorId, cvEval.CandidateId,
                cvEval.EnquiryItem.EnquiryId, cvEval.EnquiryItem.Id)==false) return null;
            }

            await _repoEval.UpdateAsync(cvEval);
            return cvEval;
        }

    /*
        public async Task<CVEvaluation> CVEvalBySup(CVEvaluation cvEval)
        {
            cvEval.ReviewedByHRSup=true;
            cvEval.ReviewedByHRSupOn=DateTime.Now;  
            await _unitOfWork.Repository<CVEvaluation>().UpdateAsync(cvEval);

            // append a record to the task of HR Executive 
            var taskHRExec = await _taskService.GetTaskEnquiryitemIdAssignedToIdTaskTypeAsync(
                cvEval.EnquiryItemId, cvEval.HRExecutiveId, enumTaskType.HRExecutiveAssignment);
            if (taskHRExec == null) 
            {
                //evalToReturn.Validated=true;
                //evalToReturn.ErrorMessage="Supervisor evaluation registered, but failed to update task of HR Executive";
                return null;
            }

            string result = "Application " + cvEval.Candidate.ApplicationNo;
            result += cvEval.HRSupReviewResult==enumItemReviewStatus.Accepted ? " approved by Supervisor, " +
                "and submitted for HRM review" : " rejected by Supervisor with the reason: " + 
                Enum.GetName(typeof(enumItemReviewStatus), cvEval.HRSupReviewResult);
        
            var taskItem = new TaskItem(taskHRExec.Id, DateTime.Now, cvEval.HRSupReviewResult
                == enumItemReviewStatus.Accepted ? 1 : 0, result,true, null);
            await _taskService.AppendTaskItemAsync(taskHRExec.Id, taskItem);
            //mark hrexec task as completed
            taskHRExec.TaskStatus = enumTaskStatus.Completed;
            await _taskService.UpdateTaskAsync(taskHRExec);

            if (cvEval.HRSupReviewResult != enumItemReviewStatus.Accepted) return cvEval;   //rejected

            if (cvEval.HRSupReviewResult == enumItemReviewStatus.Accepted)        //further processing
            {
                // if HRManager not assigned, or HRM == HRSup, then next task is DocController
                var enqitem = cvEval.EnquiryItem;
                //var enq = await _unitOfWork.Repository<Enquiry>().GetByIdAsync(cvEval.EnquiryItem.EnquiryId);
                var enq = await _context.Enquiries.Where(x=>x.Id==cvEval.EnquiryItem.EnquiryId)
                    .Select(x => new {x.EnquiryNo}).FirstOrDefaultAsync();
                //var empOwner = await _unitOfWork.Repository<Employee>().GetByIdAsync((int)cvEval.HRSupervisorId);
                var empOwner = await _context.Employees.Where(x => x.Id==cvEval.HRSupervisorId)
                    .Select(x => new {x.KnownAs}).FirstOrDefaultAsync();
                if (enqitem.AssessingHRMId == 0 || enqitem.AssessingHRMId == enqitem.AssessingSupId)
                {
                    await CreateTaskForDocControllerToSendCV((int)cvEval.HRSupervisorId, 
                        constDocControllerAdminId,  
                        cvEval.CandidateId, cvEval.EnquiryItem.EnquiryId, cvEval.EnquiryItemId);
                } else //process for HRM submission
                {
                    cvEval.HRManagerId=enqitem.AssessingHRMId;
                    // get deails required for creating a task in the name of HRManager    
                    //var empAssignedTo = await _unitOfWork.Repository<Employee>()
                    //.GetByIdAsync((int)cvEval.HRManagerId);

                    // create task for HR Manager
                    var taskForHRM = new ToDo((int)cvEval.HRSupervisorId, (int)cvEval.HRManagerId,
                        DateTime.Now, DateTime.Now.AddDays(1), empOwner.KnownAs + " has forwarded Application " +
                        cvEval.Candidate.ApplicationNo + "-" + cvEval.Candidate.FullName +
                        "for your review for requirement ref " + enq.EnquiryNo + "-" +
                        cvEval.EnquiryItem.SrNo + "-" + cvEval.EnquiryItem.CategoryName,
                        enumTaskType.HRDeptHeadAssignment, cvEval.EnquiryItem.EnquiryId, 
                        cvEval.EnquiryItem.Id);
            
                    await _taskService.CreateTaskAsync(taskForHRM);
                }
            }
            //evalToReturn.Validated=true;
            return cvEval;
            } 
        public async Task<CVEvaluationValidity> CVSubmitToHRM(CVEvaluation cvEval, 
            enumItemReviewStatus result)
        {
            var evalValidity = ValidateCVSubmissionToHRM(cvEval);
            if (!evalValidity.Validated) return evalValidity;
            cvEval.ReviewedByHRM=true;
            cvEval.ReviewedByHRMOn=DateTime.Now;
            cvEval.HRMReviewResult=result;
            await _repoEval.UpdateAsync(cvEval);

            if (result==enumItemReviewStatus.Accepted)
            {
                if (!await CreateTaskForDocControllerToSendCV((int)cvEval.HRSupervisorId, constDocControllerAdminId,  
                   cvEval.CandidateId, cvEval.EnquiryItem.EnquiryId, cvEval.EnquiryItemId))
                {
                    evalValidity.Validated=true;
                    evalValidity.ErrorMessage +="Failed to create task in the name of Doc Controller";
                }
            }
            return evalValidity;
        }
*/
        public async Task<IReadOnlyList<CVEvaluation>> GetPendingEvaluationOfAUser(int userId)
        {
            return await _context.CVEvaluations.Where(
                x => (x.HRSupervisorId==userId && x.ReviewedByHRSup==false) || 
                (x.HRManagerId == userId && x.ReviewedByHRM==false)).ToListAsync();
        }

//privates
        private async Task<Candidate> GetCandidateEntity(int candidateId)
        {
            return await _unitOfWork.Repository<Candidate>().GetByIdAsync(candidateId);
        }

    //private methods
    //validate enquiryItemId, 2 - check if alrady evaluated, 3 - check if userid assigned the enquiryitem
    //4 - chekc if assessment mandatory, 5 - check if same candidate already evaluated for same requirement
    //6 -
        private async Task<CVEvaluationValidity> ValidateCVSubmissionToSup(int EnquiryItemId,
            int CandidateId, int UserId)
        {
            var cvEval = new CVEvaluationValidity();

            // TO do *** if application setting REQUIRES HR ASSIGNMENT=false, return true
            // check if the User is tasked with the HR taskd
            if(!anyOneCanEvaluateCV)
            {
                var t = await _taskService.GetTaskAsync(EnquiryItemId, enumTaskType.HRExecutiveAssignment,
                    enumTaskStatus.NotStarted, true, UserId);
                if (t == null)
                {
                    cvEval.Validated = false;
                    throw new Exception ("User is not assigned to work on the category selected");
                    //cvEval.ErrorMessage = "The user is not assigned to work on the category";
                    //reurn cvEval;       // not tasked
                }
            }

            // check if the category item is approved + requires evaluation by HR Supervisor
            var enqItem = await _unitOfWork.Repository<EnquiryItem>().GetEntityWithSpec(
                new EnquiryItemsSpecs(EnquiryItemId));

            if (enqItem == null)
            {
                cvEval.Validated = false;
                throw new Exception("invalid category Id");
                //cvEval.ErrorMessage = "Category item invalid";
                //return null;
            }

            //2 - check if the enquiry Item requires CV EValuation

            // check if the candidate is assessed for the same requirement earlier
            var evalSpec = new CVEvaluationSpecs(EnquiryItemId, CandidateId);
            var assessed = await _unitOfWork.Repository<CVEvaluation>().GetEntityWithSpec(evalSpec);
            if (assessed != null)
            {
                cvEval.Validated = false;
                throw new Exception("the candidate has already been evaluated earlier on " + assessed.SubmittedByHRExecOn);
            }

            //check if assessment mandatory, if so, is the CV assessed
            if (enqItem.AssessmentReqd)
            {
                // is CV assessed
                var resultList = new List<enumAssessmentResult>();
                resultList.AddRange(new List<enumAssessmentResult>(){
                    enumAssessmentResult.Shortlisted_FirstOption,
                    enumAssessmentResult.Shortlisted_SecondOption});

                var assessedEntity = await _unitOfWork.Repository<Assessment>()
                    .GetEntityWithSpec(new AssessmentSpec(CandidateId, EnquiryItemId, resultList));
                if (assessedEntity == null)
                {
                    cvEval.Validated = false;
                    throw new Exception ("Assessment for the category is mandatory, but the candidate is not assessed for the category requirements");
                    //cvEval.ErrorMessage = "assessment is mandatory, but cv not assessed";
                    //return cvEval;
                }
                cvEval.Assessment = assessedEntity;
            }

            cvEval.supId = enqItem.AssessingSupId ?? 0;
            cvEval.HRMId = enqItem.AssessingHRMId ?? 0;
            cvEval.NextPhase = cvEval.supId == 0 ? "Admin" : "Sup";
            cvEval.Validated = true;

            return cvEval;
        }

        private CVEvaluationValidity ValidateCVSubmissionToHRM(CVEvaluation cvEval)
        {
            var cvValidity = new CVEvaluationValidity();

            if (cvEval.ReviewedByHRSup != true ||
                cvEval.HRSupReviewResult != enumItemReviewStatus.Accepted) 
            {
                cvValidity.Validated=false;
                cvValidity.ErrorMessage="Supervisor has not approved the evaluation";
                return cvValidity;
            }
    
            if(cvEval.ReviewedByHRSup==true)
            {
                cvValidity.Validated=false;
                cvValidity.ErrorMessage="Already reviewed by HR Manager";
                return cvValidity;
            }
            if (cvEval.HRManagerId == 0)
            {
                cvValidity.Validated=false;
                cvValidity.ErrorMessage="HR Manager not defined";
            }
            cvValidity.Validated = true;
        
            return cvValidity;
        }

         private async Task<bool> CreateTaskForDocControllerToSendCV(int ownerId, int assignedToId, 
            int candidateId, int enquiryId, int enquiryItemId)
        {
            var owner = await _context.Employees.Where(x=>x.Id==ownerId)
                .Select(x =>new {x.KnownAs}).FirstOrDefaultAsync();
            var assignedTo = await _context.Employees.Where(x=>x.Id==assignedToId)
                .Select(x =>new {x.KnownAs}).FirstOrDefaultAsync();
            var enquiryItem = await _context.EnquiryItems.Where(x => x.Id==enquiryItemId)
                .Select(x => new {x.SrNo, x.CategoryName}).FirstOrDefaultAsync();
            var enquiry = await _context.Enquiries.Where(x => x.Id == enquiryId)
                .Select(x => new {x.EnquiryNo, x.EnquiryDate, x.Customer.CustomerName}).FirstOrDefaultAsync();
            var candidate = await _context.Candidates.Where(x => x.Id == candidateId)
                .Select(x => new{x.FullName, x.Gender, x.ApplicationNo, x.PPNo, x.AadharNo}).FirstOrDefaultAsync();
            // create task for Doc Controller - Admin
            var taskForDocControllerAdmin = new ToDo(ownerId, assignedToId, DateTime.Now,
                DateTime.Now.AddDays(1), owner.KnownAs + " has forwarded Application " +
                candidate.ApplicationNo + "-" + candidate.FullName +
                "for forwarding to client " + enquiry.CustomerName + " for requirement ref " +
                enquiry.EnquiryNo + "-" + enquiryItem.SrNo + "-" + enquiryItem.CategoryName,
                enumTaskType.HRSupervisorAssignment, enquiryId, enquiryItemId);

            var tasked = await _taskService.CreateTaskAsync(taskForDocControllerAdmin);
            
            return tasked == null ? false : true;
        }


    }
}