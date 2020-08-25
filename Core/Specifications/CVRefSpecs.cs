using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Entities.Admin;
using Core.Enumerations;

namespace Core.Specifications
{
    public class CVRefSpecs : BaseSpecification<CVRef>
    {
        public CVRefSpecs(CVRefParam param) 
        : base(x => 
                (
                    (!param.Id.HasValue || x.Id == param.Id) && 
                    (!param.EnquiryItemId.HasValue || x.EnquiryItemId == param.EnquiryItemId) && 
                    (!param.CandidateId.HasValue || x.CandidateId == param.CandidateId) && 
                    (!param.ApplicationNo.HasValue || x.ApplicationNo == param.ApplicationNo) && 
                    (!param.HRExecutiveId.HasValue || x.HRExecutiveId == param.HRExecutiveId) && 
                    (!param.ReferredOn.HasValue || DateTime.Compare(
                        x.DateForwarded.Date, param.ReferredOn.Value.Date)==0)
                ))
        {
            AddOrderBy(x => x.DateForwarded);
        }

        public CVRefSpecs(int candidateId): base(x => x.CandidateId==candidateId)
        {
            AddOrderBy(x => x.DateForwarded);
        }

        public CVRefSpecs(int candidateId, int enquiryItemId)
            : base(x => x.CandidateId==candidateId && x.EnquiryItemId==enquiryItemId)
        {
            AddOrderBy(x => x.DateForwarded);
        }

         public CVRefSpecs(List<int> enquiryItemIds)
            : base(x => enquiryItemIds.Contains(x.EnquiryItemId))
        {
            AddOrderBy(x => x.DateForwarded);
        }

        public CVRefSpecs(List<int> enquiryItemIds, enumSelectionResult result)
            : base(x => enquiryItemIds.Contains(x.EnquiryItemId) && x.RefStatus==result)
        {
            AddOrderBy(x => x.DateForwarded);
        }

        public CVRefSpecs(DateTime dateFrom, DateTime dateUpto)
            : base(x => ((DateTime.Compare(x.DateForwarded.Date, dateFrom.Date)>=0) &&
                DateTime.Compare(x.DateForwarded.Date, dateUpto.Date) <=0))
        {
            AddOrderBy(x => x.DateForwarded);
        }
    }
}