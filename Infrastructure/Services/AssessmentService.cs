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
            /*
            var catExist = await _context.CandidateCategories.Where(x => x.CandId==CandidateId)
                .Select(x => new { catid =   _context.EnquiryItems
                .Where(x => x.Id == EnquiryItemId)
                .Select(x => x.CategoryItemId).Take(1).SingleOrDefault(),
                 }).ToListAsync();

            var catExist = await _context.EnquiryItems.Where(x => x.Id == EnquiryItemId)
                .Select(x => new { catid = _context.EnquiryItems
                .Where(x => x.Id == EnquiryItemId).Select(x => x.CategoryItemId).Take(1).SingleOrDefault(),
                 }).ToListAsync();

            
             var enq = await _context.EnquiryItems.Where(x => x.Id == EnquiryItemId)
                .Select(x => new { categoryId=x.CategoryItemId, EnquiryId= x.EnquiryId})
                .FirstOrDefaultAsync();
            var Qs = await _context.AssessmentQsBank.Where(x => x.IsStandardQuestion==false
                && x.CategoryId == enq.categoryId).ToListAsync();
       */

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
                .OrderBy(x => x.AssessmentParameter).ToListAsync();
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
            var b = await OkToCreateAssessment(enquiryItemId, candidateId);
            if (b != true) return null;

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
            //var list = from p in assessment.AssessmentItems where p.Assessed==true select (p=>p.AssessmentItems);
            var list = assessment.AssessmentItems.Where(x=>x.Assessed==true);
            var totPoints = list.Sum(x=>x.MaxPoints);
            var totGiven = list.Sum(x=>x.PointsAllotted);
            var GradeString = GetGrade(totPoints, totGiven);
            assessment.Grade= 100*totGiven/totPoints;
            assessment.GradeString= GetGrade(totPoints, totGiven);

            return await _unitOfWork.Repository<Assessment>().UpdateAsync(assessment);
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
        //domains - deleted
        /*
                public async Task<IReadOnlyList<DomainSub>> AddDomains(IReadOnlyList<strObject> domainList)
                {
                    var dmnList = new List<DomainSub>();
                    foreach(var dmn in domainList)
                    {
                        dmnList.Add(new DomainSub(dmn.Name));
                    }
                    var domainsAdded = await _unitOfWork.Repository<DomainSub>().AddListAsync(dmnList);
                    if(domainsAdded==null || domainsAdded.Count==0) 
                        throw new Exception("Failed to add the domains");
                    return domainsAdded;
                }

                public async Task<int> DeleteDomains(List<DomainSub> domainSubs)
                {
                    return await _unitOfWork.Repository<DomainSub>().DeleteListAsync(domainSubs);
                }

                public async Task<int> UpdateDomains(List<DomainSub> domainList)
                {
                    return await _unitOfWork.Repository<DomainSub>().UpdateListAsync(domainList);
                }

                public async Task<IReadOnlyList<DomainSub>> GetDomainList()
                {
                    return await _context.DomainSubs.OrderBy(x=>x.DomainSubName).ToListAsync();
                }
            */

        public async Task<bool> OkToCreateAssessment(int enquiryItemId, int candidateId)
        {

            var exist = await _context.AssessmentQs.Where(x => x.EnquiryItemId == enquiryItemId)
                .SingleOrDefaultAsync();
            if (exist == null) throw new Exception("Before a candidate can be assessed against a " +
                  "requriement, assessment questions need to be designed for the requirement. " +
                  "If the assessment questions do not need to be customised to customer job descriptions, " +
                  "you may consider copying the standard assessment questions");

            //does the enquiry item has assessment
            var qs = await _context.Assessments.Where(x => x.EnquiryItemId == enquiryItemId
                && x.CandidateId == candidateId)
                .Select(x => new { by = x.AssessedBy, on = x.AssessedOn })
                .FirstOrDefaultAsync();
            if (qs != null) throw new Exception("the candidate has already been " +
                  "assessed for the same requirement on " + qs.on + " by " + qs.by +
                  ". If you want to edit the assessment, choose the edit option");

            return true;
        }


        private string GetGrade(int totalPoints, int totalGiven)
        {
            int Pt = 100 * totalGiven/ totalPoints;

            if (Pt >=90) {return "A";}
            else if(Pt >=75) {return "B";}
            else if(Pt >=50) {return "C";}
            else if(Pt >=40) {return "D";}
            else {return "E";}
        }
    }
}