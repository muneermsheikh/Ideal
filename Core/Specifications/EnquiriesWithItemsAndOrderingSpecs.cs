using System;
using System.Linq.Expressions;
using Core.Entities.EnquiryAggregate;

namespace Core.Specifications
{
    public class EnquiriesWithItemsAndOrderingSpecs : BaseSpecification<Enquiry>
    {
        public EnquiriesWithItemsAndOrderingSpecs(string email): base (o => o.BuyerEmail == email)
        {
            AddInclude(o => o.EnquiryItems);
            AddOrderByDescending(o => o.EnquiryDate);
        }

        public EnquiriesWithItemsAndOrderingSpecs(int Id, string email): 
            base (o => o.BuyerEmail == email && o.Id == Id)
        {
            AddInclude(o => o.EnquiryItems);
        }
    }
}