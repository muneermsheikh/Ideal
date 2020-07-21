using System;
using System.Linq.Expressions;
using Core.Entities.HR;

namespace Core.Specifications
{
    public class AssessmentSpec : BaseSpecification<Assessment>
    {
        public AssessmentSpec(AssessmentParam aParams) 
            : base( x => x.CandidateId == aParams.CandidateId && 
                x.EnquiryItemId == aParams.EnquiryItemId)
        {
            if (aParams.IncludeItems) 
            {
                AddInclude(x => x.AssessmentItems);
            }
        }

        public AssessmentSpec(int CandidateId, int EnquiryItemId)
            : base( x => x.CandidateId == CandidateId && x.EnquiryItemId == EnquiryItemId)
        {
            AddInclude(x => x.AssessmentItems);
        }
        public AssessmentSpec(int EnquiryItemId) : base(x => x.EnquiryItemId == EnquiryItemId)
        {
            AddOrderBy(x => x.AssessedOn);
        }

        public AssessmentSpec(string dummy, int CandidateId) : base(x => x.CandidateId == CandidateId)
        {
            AddOrderBy(x => x.AssessedOn);
        }
    }
}