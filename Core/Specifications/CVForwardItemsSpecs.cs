using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Entities.Admin;
using Core.Enumerations;

namespace Core.Specifications
{
    public class CVForwardItemsSpecs : BaseSpecification<CVForwardItem>
    {
        public CVForwardItemsSpecs(CVForwardItemParam param)
            :  base( x => 
            (
                (!param.EnquiryItemId.HasValue || x.EnquiryItemId == param.EnquiryItemId) &&
                (!param.EnquiryId.HasValue || x.EnquiryId == param.EnquiryId) &&
                (!param.CandidateId.HasValue || x.CandidateId == param.CandidateId) 
            ))
        {            
            ApplyPaging(param.PageSize * (param.PageIndex-1), param.PageSize);
        }


        public CVForwardItemsSpecs(int candidateId)
            :  base( x => x.CandidateId==candidateId)
        {
        }

        public CVForwardItemsSpecs(string dummy, int enquiryId)
            :  base( x => x.EnquiryId == enquiryId)
        {
        }

    }
}