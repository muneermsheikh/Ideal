using System;
using System.Linq.Expressions;
using Core.Entities.EnquiryAggregate;

namespace Core.Specifications
{
    public class EnquirySpecs : BaseSpecification<Enquiry>
    {
        public EnquirySpecs(string email): base (o => o.BuyerEmail == email)
        {
            AddInclude(o => o.EnquiryItems);
            AddOrderByDescending(o => o.EnquiryDate);
        }

        public EnquirySpecs(int Id, string email): 
            base (o => o.BuyerEmail == email && o.Id == Id)
        {
            AddInclude(o => o.EnquiryItems);
        }

        public EnquirySpecs(int enquiryId): 
            base (o => o.Id == enquiryId)
        {
        }

    }
}