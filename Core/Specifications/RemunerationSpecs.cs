using System;
using System.Linq.Expressions;
using Core.Entities.EnquiryAggregate;

namespace Core.Specifications
{
    public class RemunerationSpecs : BaseSpecification<Remuneration>
    {
        public RemunerationSpecs()
        {
        }

        public RemunerationSpecs(int enquiryItemId) : base(x => x.EnquiryItemId == enquiryItemId)
        {
        }

        public RemunerationSpecs(string dummy, int enquiryId) : base(x => x.EnquiryId == enquiryId)
        {
        }
    }
}