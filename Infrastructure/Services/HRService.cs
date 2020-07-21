using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.EnquiryAggregate;
using Core.Entities.HR;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class HRService : IHRService
    {
        private readonly IGenericRepository<Assessment> _assessRepo;
        private readonly IUnitOfWork _unitOfWork;
        public HRService(IGenericRepository<Assessment> assessRepo, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _assessRepo = assessRepo;
        }

        public async Task<Assessment> GetCandidateAssessmentSheet(
                int CandidateId, int EnquiryItemId, int UserId, string UserDisplayName)
        {
            // create assessment sheet for the candidate if one does not exist
            var spec = new AssessmentSpec(CandidateId, EnquiryItemId);
            var assessSheet = await _assessRepo.GetEntityWithSpec(spec);

            if (assessSheet == null)
            {
                var itemDetails = await _unitOfWork.Repository<EnquiryItem>().GetByIdAsync(EnquiryItemId);
                var enqId = itemDetails.EnquiryId;
                var enqDetails = await _unitOfWork.Repository<Enquiry>().GetByIdAsync(enqId);
                var CustNameAndCity = enqDetails.Customer.CustomerName + ", " + enqDetails.Customer.CityName;
                var categoryNameAndRef = enqDetails.EnquiryNo + "-" + itemDetails.SrNo + "-" + itemDetails.CategoryName;

                // Assessment contains List<AssessmentItem> object - create it
                // retrieve assessment questions defined for the enquriy item
                var specQ = new AssessmentQSpec(EnquiryItemId);
                if (await _unitOfWork.Repository<AssessmentQ>().RecordExists(specQ)) return null;
                var Qs = await _unitOfWork.Repository<AssessmentQ>().GetEntityListWithSpec(specQ);
                if (Qs == null) return null;
                //transfer the questions to list<AssessmentItem>
                var assItems = new List<AssessmentItem>();
                foreach (var q in Qs)
                {
                    var assItem = new AssessmentItem(q.QuestionNo, q.IsMandatory, q.DomainSubject, 
                        q.AssessmentParameter, q.Question, q.MaxPoints);
                    assItems.Add(assItem);
                }
                var listItems = await _unitOfWork.Repository<AssessmentItem>().AddListAsync(assItems);
                // Create the parent object
                var assHeader = new Assessment(CustNameAndCity, EnquiryItemId, categoryNameAndRef, CandidateId, UserDisplayName, assItems);
                assessSheet = await _unitOfWork.Repository<Assessment>().AddAsync(assHeader);
                
                if (assessSheet==null) return null;
            }
            return assessSheet;
        }

        public async Task<Assessment> EditAssessment(Assessment assessment)
        {
            // todo - validation of assessment
            // return await _unitOfWork.Repository<Assessment>().UpdateAsync(assessment);
            return await _assessRepo.UpdateAsync(assessment);
        }

        public async Task<IReadOnlyList<Assessment>> GetAssessmentList(int EnquiryItemId)
        {
            var spec = new AssessmentSpec(EnquiryItemId);
            return await _assessRepo.GetEntityListWithSpec(spec);
        }

    }
}