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
    }
}