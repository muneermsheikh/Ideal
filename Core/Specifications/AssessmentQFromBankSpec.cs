using System;
using System.Linq.Expressions;
using Core.Entities.Masters;

namespace Core.Specifications
{
    public class AssessmentQFromBankSpec : BaseSpecification<AssessmentQBank>
    {
        public AssessmentQFromBankSpec(AssessmentQParams qParams) 
            : base( x =>
                (x.IsStandardQuestion == qParams.IsStandardQuestion) &&
                (string.IsNullOrEmpty(qParams.AssessmentParameter) || 
                    x.AssessmentParameter.ToLower().Contains(qParams.AssessmentParameter)) &&
                (string.IsNullOrEmpty(qParams.DomainSub) || 
                    x.AssessmentParameter.ToLower().Contains(qParams.DomainSub)) &&
                (!qParams.DomainSubId.HasValue || 
                    x.DomainSubId == qParams.DomainSubId)
                )
        {
            AddOrderBy(x => x.SrNo);
        }
        public AssessmentQFromBankSpec(bool IsStandardQ) 
            : base( x => x.IsStandardQuestion == IsStandardQ)
        {
            AddOrderBy(x => x.SrNo);
        }
    }
}