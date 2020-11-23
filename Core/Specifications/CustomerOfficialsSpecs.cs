using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Entities.Admin;

namespace Core.Specifications
{
    public class CustomerOfficialsSpecs : BaseSpecification<CustomerOfficial>
    {
        public CustomerOfficialsSpecs(int customerId, string officialName) 
            : base(x => (
                 (string.IsNullOrEmpty(officialName) || 
                        x.Name.ToLower().Contains(officialName) &&
                        x.IsValid.ToLower() == "t" &&
                        x.CustomerId == customerId)
            ))
        {
        }
        public CustomerOfficialsSpecs(int customerId) 
            : base(x => x.CustomerId == customerId)
        {
        }

        public CustomerOfficialsSpecs(IReadOnlyList<IdInt> officialIds, string isValid) 
            :base( x => officialIds.Select(x => x.Id).Contains(x.Id) && x.IsValid.ToLower()==isValid.ToLower())
        {
        }
        
    }
}