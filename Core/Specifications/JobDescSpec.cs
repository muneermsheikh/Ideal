using System;
using System.Linq.Expressions;
using Core.Entities.EnquiryAggregate;

namespace Core.Specifications
{
    public class JobDescSpec : BaseSpecification<JobDesc>
    {
        public JobDescSpec(int enquiryItemId) : base(x => x.EnquiryItemId == enquiryItemId)
        {
        }

        public JobDescSpec(string dummy, int enquiryId): base(x => x.EnquiryId == enquiryId)
        {
        }
    }
}