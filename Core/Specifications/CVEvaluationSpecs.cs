using System;
using System.Linq.Expressions;
using Core.Entities.HR;

namespace Core.Specifications
{
    public class CVEvaluationSpecs : BaseSpecification<CVEvaluation>
    {
        
        public CVEvaluationSpecs(string dummy, int id)
            : base (x => x.Id == id)
        {
        }

        public CVEvaluationSpecs(int candidateId)
            : base (x => x.CandidateId == candidateId)
        {
        }

        public CVEvaluationSpecs(int enquiryItemId, int candidateId)
            : base (x => x.CandidateId == candidateId && x.EnquiryItemId == enquiryItemId)
        {
        }

        public CVEvaluationSpecs(int enquiryItemId, string dummy)
            : base (x => x.EnquiryItemId == enquiryItemId)
        {
        }
        
        public CVEvaluationSpecs(CVEvaluationParam cvParam) 
        :  base( x => (
                (!cvParam.ApplicationNo.HasValue || x.ApplicationNo == cvParam.ApplicationNo) &&
                (!cvParam.HRExecutiveId.HasValue || x.HRExecutiveId == cvParam.HRExecutiveId) &&
                (!cvParam.HRSupervisorId.HasValue || x.HRSupervisorId == cvParam.HRSupervisorId) &&
                (!cvParam.HRManagerId.HasValue || x.HRManagerId == cvParam.HRManagerId) &&
                (!cvParam.SubmittedByHRExecOn.HasValue || DateTime.Compare(
                    x.SubmittedByHRExecOn.Date, (DateTime)cvParam.SubmittedByHRExecOn) == 0) &&
                (!cvParam.ReviewedByHRSupOn.HasValue || DateTime.Compare(
                    x.ReviewedByHRSupOn.Value.Date, (DateTime)cvParam.ReviewedByHRSupOn) == 0) &&
                (!cvParam.ReviewedByHRMOn.HasValue || DateTime.Compare(
                    x.ReviewedByHRMOn.Value.Date, (DateTime)cvParam.ReviewedByHRMOn) == 0) &&
                (!cvParam.HRSupReviewResult.HasValue || 
                    x.HRSupReviewResult == cvParam.HRSupReviewResult) &&
                (!cvParam.HRMReviewResult.HasValue || 
                    x.HRMReviewResult == cvParam.HRMReviewResult) 
                )
            )
        {
            if (cvParam.PageSize != 0)
            {
                ApplyPaging(cvParam.PageSize, cvParam.PageSize * (cvParam.PageIndex-1));

                if (!string.IsNullOrEmpty(cvParam.Sort))
                {
                    switch (cvParam.Sort)
                    {
                        case "FullNameAsc":
                            AddOrderBy(x => x.FullName);
                            break;
                        case "FullNameDesc":
                            AddOrderByDescending(x => x.FullName);
                            break;
                        case "CategoryNameAsc":
                            AddOrderBy(x => x.EnquiryItem.CategoryName);
                            break;
                        case "CategoryNameDesc":
                            AddOrderByDescending(x => x.EnquiryItem.CategoryName);
                            break;
                        case "SubmittedByHRExecOnAsc":
                            AddOrderBy(x => x.SubmittedByHRExecOn);
                            break;
                        case "SubmittedByHRExecOnDesc":
                            AddOrderBy(x => x.SubmittedByHRExecOn);
                            break;
                        case "ReviewedByHRSupOnAsc":
                            AddOrderBy(x => x.ReviewedByHRSupOn);
                            break;
                        case "ReviewedByHRSUpOnDesc":
                            AddOrderBy(x => x.ReviewedByHRSupOn);
                            break;
                        default:
                            AddOrderByDescending(x => x.SubmittedByHRExecOn);
                            break;
                    }
                }
            }

        }

    }
}