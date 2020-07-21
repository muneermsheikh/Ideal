using System;
using System.Linq.Expressions;
using Core.Entities.HR;

namespace Core.Specifications
{
    public class AssessmentQSpec : BaseSpecification<AssessmentQ>
    {
        public AssessmentQSpec(int enquiryItemId) 
            :  base( x => x.EnquiryItemId == enquiryItemId)
        {
            AddOrderBy(x => x.QuestionNo);
        }
    }
}