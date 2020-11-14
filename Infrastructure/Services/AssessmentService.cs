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
    public class AssessmentService : IAssessmentService
    {
        private readonly ITaskService _taskService;
        private readonly ATSContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEnquiryService _enqService;
        public AssessmentService(ATSContext context, ITaskService taskService, 
            IUnitOfWork unitOfWork, IEnquiryService enqService)
        {
            _enqService = enqService;
            _unitOfWork = unitOfWork;
            _context = context;
            _taskService = taskService;
        }

//assessmentQForEnquiryItem
        public async Task<IReadOnlyList<AssessmentQ>> CopyStddQToAssessmentQOfEnquiryItem(int EnquiryItemId)
        {
            var qs = await _context.AssessmentQs.Where(x => x.EnquiryItemId==EnquiryItemId).ToListAsync();
            if (qs != null && qs.Count > 0) throw new Exception ("The selected enquiry item already has assessment questions defined");
            
            var QBank = await _context.AssessmentQsBank.Where(x => x.IsStandardQuestion == true)
                .OrderBy(x => x.SrNo).ToListAsync();
            if (QBank == null || QBank.Count == 0) throw new Exception("No standard assessment questions exist on record");
            int enquiryid = await _context.EnquiryItems.Where(x => x.Id == EnquiryItemId).Select(x => x.EnquiryId).FirstOrDefaultAsync();
            var Qs = new List<AssessmentQ>();
            int qNo = 0;
            foreach (var Q in QBank)
            {
                Qs.Add(new AssessmentQ(EnquiryItemId, enquiryid,
                    ++qNo, Q.AssessmentParameter, Q.Question, Q.MaxPoints));
            }
            return await _unitOfWork.Repository<AssessmentQ>().AddListAsync(Qs);
        }

        public async Task<IReadOnlyList<AssessmentQ>> CopyQToAssessmentQOfEnquiryItem(int EnquiryItemId)
        {
            var qs = await _context.AssessmentQs.Where(x => x.EnquiryItemId==EnquiryItemId).ToListAsync();
            if (qs != null && qs.Count > 0) throw new Exception ("The selected enquiry item already has assessment questions defined");
            
            var Qs = await _context.AssessmentQsBank
                .Join(_context.EnquiryItems, e => e.CategoryId, q => q.CategoryItemId,
                (e, q) => new { e, q })
                .Where(m => m.e.Id == EnquiryItemId)
                .Select(m => new
                {
                    enqItemId = m.q.Id,
                    enqId = m.q.EnquiryId,
                    assParm = m.e.AssessmentParameter,
                    question = m.e.Question,
                    maxPt = m.e.MaxPoints
                }).ToListAsync();

            if (Qs == null || Qs.Count() == 0) throw new Exception("The question bank does not contain any questions for the category " +
                " in the enquiry item");
            var QList = new List<AssessmentQ>();
            int qNo = 0;
            foreach (var Q in Qs)
            {
                QList.Add(new AssessmentQ(Q.enqItemId, Q.enqId, ++qNo, Q.assParm, Q.question, Q.maxPt));
            }
            return await _unitOfWork.Repository<AssessmentQ>().AddListAsync(QList);
        }

        public async Task<IReadOnlyList<AssessmentQ>> GetAssessmentQsOfEnquiryItem(int enquiryItemId)
        {
            var Qs = await _context.AssessmentQs.Where(x => x.EnquiryItemId == enquiryItemId)
                .OrderBy(x => x.QuestionNo).ToListAsync();
            return Qs;
        }

        public async Task<IReadOnlyList<AssessmentQ>> GetAssessmentQsOfEnquiry(int enquiryId)
        {
            var Qs = await _context.AssessmentQs.Where(x => x.EnquiryId == enquiryId)
                .OrderBy(x => x.EnquiryItemId). OrderBy(x => x.QuestionNo).ToListAsync();
            return Qs;
        }


        public async Task<int> DeleteStddQOfEnquiryItem(List<AssessmentQ> assessmentQs)
        {
            return await _unitOfWork.Repository<AssessmentQ>()
                .DeleteListAsync(assessmentQs);
        }

        public async Task<int> UpdateQsOfEnquiryItem(List<AssessmentQ> assessmentQs)
        {
            var affected = await _unitOfWork.Repository<AssessmentQ>()
                .UpdateListAsync(assessmentQs);
            return affected;
        }

//Assessment of candidates
        public async Task<Assessment> CreateAssessment(int enquiryItemId, int candidateId, string loggedInUserName)
        {
            //validate AssessmentCreation
            if (!await CheckIfCandidateAlreadyAssessed(enquiryItemId, candidateId)) return null;

            var qs = await _context.AssessmentQs.Where(x => x.EnquiryItemId == enquiryItemId)
                .OrderBy(x => x.QuestionNo).ToListAsync();
            if (qs==null || qs.Count==0) throw new Exception("No assessment questions on record for the selected requirement");
            var ass = new Assessment();
            var lstItems = new List<AssessmentItem>();
            foreach (var q in qs)
            {
                lstItems.Add(new AssessmentItem(q.QuestionNo, q.IsMandatory,
                    q.AssessmentParameter, q.Question, q.MaxPoints));
            }

            var itemdetails = await _enqService.GetDetailsFromEnquiryItemId(enquiryItemId);

            ass.CandidateId=candidateId;
            ass.EnquiryItemId = enquiryItemId;
            ass.CustomerNameAndCity = itemdetails.CustomerName + ", " + itemdetails.CityName;
            ass.CategoryNameAndRef = itemdetails.CategoryRef;
            ass.AssessedOn = DateTime.Now;
            ass.AssessedBy = loggedInUserName;
            ass.AssessmentItems = lstItems;

            return ass;
        }

        public async Task<Assessment> UpdateAssessment(Assessment assessment)
        {
            if (!await CheckAssessmentData(assessment)) return null;
            var assessUpdated= await UpdateAssessmentGrades(assessment);
            return await _unitOfWork.Repository<Assessment>().AddAsync(assessUpdated);
        }

        public async Task<int> DeleteAssessment(Assessment assessment)
        {
            return await _unitOfWork.Repository<Assessment>().DeleteAsync(assessment);
        }

        public async Task<IReadOnlyList<Assessment>> GetAssessmentBySpec(AssessmentParam assessmentParam)
        {
            return await _unitOfWork.Repository<Assessment>().GetEntityListWithSpec(new AssessmentSpec(assessmentParam));
        }

//assessment Q Bank
        public async Task<IReadOnlyList<AssessmentQBank>> AddQListToAssessmentQBank(
            IReadOnlyList<AssessmentQBank> Qs)
        {
            return await _unitOfWork.Repository<AssessmentQBank>().AddListAsync(Qs);
        }

        public async Task<int> UpdateAssessmentQsBank(List<AssessmentQBank> Qs)
        {
            return await _unitOfWork.Repository<AssessmentQBank>().UpdateListAsync(Qs);
        }

        public async Task<int> deleteAssessmentQsBank(List<AssessmentQBank> Qs)
        {
            return await _unitOfWork.Repository<AssessmentQBank>().DeleteListAsync(Qs);
        }

        public async Task<IReadOnlyList<AssessmentQBank>> GetQBankForACategory(int categoryId)
        {
            var q = await _context.AssessmentQsBank
                .Where(x => x.CategoryId == categoryId)
                .OrderBy(x => x.SrNo).ToListAsync();
            if (q == null || q.Count == 0) throw new Exception("No questions exist belonging to the selected category");
            return q;
        }

        public async Task<IReadOnlyList<AssessmentQBank>> GetQBankForEnquiryItem(int enquiryItemId)
        {
            var categoryId = await _context.EnquiryItems.Where(x => x.Id == enquiryItemId)
                .Select(x => x.CategoryItemId).FirstOrDefaultAsync();
            if (categoryId == 0) throw new Exception("Invalid Enquiry Item");

            var q = await _context.AssessmentQsBank.Where(x => x.CategoryId == categoryId)
                .OrderBy(x => x.SrNo).ToListAsync();

            if (q == null || q.Count == 0) throw new Exception("No questions exist belonging to the selected category");
            return q;
        }
        public async Task<IReadOnlyList<AssessmentQBank>> GetQBank_All()
        {
            var q = await _context.AssessmentQsBank.Where(x => x.CategoryId != 0)
                .OrderBy(x => x.CategoryId).ThenBy(x => x.SrNo).ToListAsync();
            if (q == null || q.Count == 0) throw new Exception("Question Bank is empty");
            return q;
        }

        public async Task<bool> CheckAssessmentData(Assessment assessment)
        {
            //There is no guarantee only valid data will come from clients. make following checks

            if(Enum.IsDefined(typeof(enumAssessmentResult), assessment.Result)==false)
            {
                throw new Exception("Invalid assessment result value. check enumAssessmentResult " +
                    "for acceptable values");
            }

            if (assessment.Result==0) throw new Exception("assessment result not provided");

            if(string.IsNullOrEmpty(assessment.AssessedBy)) throw new Exception("name of Assessor not available");
            if(assessment.CandidateId==0) throw new Exception("candidate id not provided");
            if(assessment.EnquiryItemId==0) throw new Exception ("requirement reference not provided");
            if(assessment.AssessmentItems.Count==0) throw new Exception("Assessment items not present");

            var exist = await _context.AssessmentQs.Where(x => x.EnquiryItemId == assessment.EnquiryItemId)
                .ToListAsync();
            if (exist == null || exist.Count == 0) throw new Exception("Before a candidate can be assessed against a " +
                  "requriement, assessment questions need to be designed for the requirement. " +
                  "If the assessment questions do not need to be customised to customer job descriptions, " +
                  "you may consider copying the standard assessment questions");

            //does the candidte possess category skills as per the requirement
            //- check if candidatecategories.catId==enquriyitem.categoryid
            var qry = await (from e in _context.CandidateCategories 
                        join i in _context.EnquiryItems
                        on e.CategoryId equals i.CategoryItemId 
                        where i.Id == assessment.EnquiryItemId && e.CandidateId==assessment.CandidateId
                        select e.Id ).FirstOrDefaultAsync();
            
            if (qry==0) throw new Exception("Candidate category skills are not shared with the " +
                "category skill required by the Order Item category.");
            
            if (await CheckIfCandidateAlreadyAssessed(assessment.CandidateId, assessment.EnquiryItemId)) return false;

            return true;
        }


        public async Task<Assessment> UpdateAssessmentGrades(Assessment assessment)
        {
            // items is from DB
            var QFromDB = await _context.AssessmentQs.Where(x=>x.EnquiryItemId==assessment.EnquiryItemId)
                .OrderBy(x=>x.QuestionNo).ToListAsync();
           
            bool found=false;
            foreach(var Qdb in QFromDB)
            {
                foreach(var itm in assessment.AssessmentItems)
                {
                    found=false;
                    if(itm.QuestionNo==Qdb.QuestionNo)
                    {
                        itm.Question=Qdb.Question;
                        itm.AssessmentParameter=Qdb.AssessmentParameter;
                        itm.IsMandatory=Qdb.IsMandatory;
                        itm.MaxPoints=Qdb.MaxPoints;
                        found=true;
                        break;
                    }
                }
                if(!found && Qdb.IsMandatory) throw new Exception("Question No." + Qdb.QuestionNo +
                    "(" + Qdb.Question +") not included in client response");
            }

            //var list = from p in assessment.AssessmentItems where p.Assessed==true select (p=>p.AssessmentItems);
            var totGiven = assessment.AssessmentItems.Where(x=>x.Assessed==true).Sum(x => x.PointsAllotted);
            var totPoints = assessment.AssessmentItems.Where(x=>x.Assessed==true).Sum(x => x.MaxPoints);
            assessment.Grade= 100*totGiven/totPoints;
            assessment.GradeString= "Grade " + GetGrade(totPoints, totGiven);

            return assessment;
        }
        private string GetGrade(int totalPoints, int totalGiven)
        {
            if (totalPoints==0) throw new Exception("total Points not retrieved");
            int Pt = 100 * totalGiven/ totalPoints;

            if (Pt >=90) {return "A";}
            else if(Pt >=75) {return "B";}
            else if(Pt >=50) {return "C";}
            else if(Pt >=40) {return "D";}
            else {return "E";}
        }

        public async Task<bool> CheckIfCandidateAlreadyAssessed(int candidateId, int enquiryItemId)
        {
            //does the enquiry item has assessment
            var qs = await _context.Assessments.Where(x => x.EnquiryItemId == enquiryItemId
                && x.CandidateId == candidateId)
                .Select(x => new { by = x.AssessedBy, on = x.AssessedOn })
                .FirstOrDefaultAsync();
            if (qs != null) throw new Exception("the candidate has already been " +
                  "assessed for the same requirement on " + qs.on + " by " + qs.by +
                  ". If you want to edit the assessment, choose the edit option");
            return false;
        }

    }
}