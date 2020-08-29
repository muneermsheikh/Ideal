using System;
using System.Linq.Expressions;
using Core.Entities.Masters;

namespace Core.Specifications
{
    public class AssessmentQFromBankSpec : BaseSpecification<AssessmentQBank>
    {
        public AssessmentQFromBankSpec(AssessmentQFromBankParams qParams) 
            : base( x =>
                (x.IsStandardQuestion == qParams.IsStandardQuestion) &&
                (string.IsNullOrEmpty(qParams.AssessmentParameter) || 
                    x.AssessmentParameter.ToLower().Contains(qParams.AssessmentParameter)) &&
                (!qParams.CategoryId.HasValue) || x.CategoryId == qParams.CategoryId)
        {
            AddOrderBy(x => x.CategoryId);
            AddOrderBy(x => x.SrNo);
        }
        public AssessmentQFromBankSpec(bool IsStandardQ) 
            : base( x => x.IsStandardQuestion == IsStandardQ)
        {
            AddOrderBy(x => x.SrNo);
        }
        public AssessmentQFromBankSpec(int categoryId)
            :base( x => x.CategoryId == categoryId)
        {
            AddOrderBy(x => x.SrNo);
        }
    }
}