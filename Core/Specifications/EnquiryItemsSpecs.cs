using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;
using System.Linq;

namespace Core.Specifications
{
    public class EnquiryItemsSpecs : BaseSpecification<EnquiryItem>
    {
        public EnquiryItemsSpecs()
        {
        }

        public EnquiryItemsSpecs(int enquiryId, string reviewStatus) 
            : base(x => x.EnquiryId == enquiryId &&  x.ReviewStatus == reviewStatus)
        {
            AddOrderBy(x => x.SrNo);
        }

        public EnquiryItemsSpecs(int enquiryId, bool includeAll) 
            : base(x => x.EnquiryId == enquiryId)
        {
            if (includeAll)
            {
                AddInclude(x => x.JobDesc);
                AddInclude(x => x.Remuneration);
            }
            AddOrderBy(x => x.SrNo);
        }

        public EnquiryItemsSpecs(int enquiryItemId, string itemStatus, 
            bool HRSupAssigned) 
            : base(x => (x.Id == enquiryItemId && x.ReviewStatus == itemStatus) &&
                (x.AssessingSupId.HasValue) )
        {
        }
        
        public EnquiryItemsSpecs(int enquiryItemId) 
            : base(x => x.Id == enquiryItemId)
        {
            AddInclude(x => x.JobDesc);
            AddInclude(x => x.Remuneration);
        }

        public EnquiryItemsSpecs(int SerialNo, int enquiryId)
            : base(x => x.SrNo == SerialNo && x.EnquiryId == enquiryId)
        {
        }

        public EnquiryItemsSpecs(IReadOnlyList<IdInt> enquiryItemIds, int enquiryId, 
            string reviewStatus, bool includeAll)
            : base (x => enquiryItemIds.Select(x=>x.Id).Contains(x.Id) &&
                x.EnquiryId == enquiryId && x.ReviewStatus == reviewStatus)
        {
            if (includeAll)
            {
                AddInclude(x => x.JobDesc);
                AddInclude(x => x.Remuneration);
            }
        }
       public EnquiryItemsSpecs(int enquiryId, int enquiryItemId, string EnqAndItemId)
            : base (x => x.EnquiryId == enquiryId && x.Id == enquiryItemId)
        {
        }

    }
}