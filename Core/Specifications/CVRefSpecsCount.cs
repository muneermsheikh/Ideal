using System;
using System.Collections.Generic;
using Core.Entities.Admin;
using Core.Enumerations;

namespace Core.Specifications
{
    public class CVRefSpecsCount: BaseSpecification<CVRef>
    {
        public CVRefSpecsCount(CVRefParam param) 
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
        }

        public CVRefSpecsCount(int candidateId): base(x => x.CandidateId==candidateId)
        {
        }

        public CVRefSpecsCount(int candidateId, int enquiryItemId)
            : base(x => x.CandidateId==candidateId && x.EnquiryItemId==enquiryItemId)
        {
        }

        public CVRefSpecsCount(List<int> enquiryItemIds)
            : base(x => enquiryItemIds.Contains(x.EnquiryItemId))
        {
        }

        
        public CVRefSpecsCount(List<int> enquiryItemIds, enumSelectionResult result)
            : base(x => enquiryItemIds.Contains(x.EnquiryItemId) && x.RefStatus==result)
        {
        }

        public CVRefSpecsCount(DateTime dateFrom, DateTime dateUpto)
            : base(x => ((DateTime.Compare(x.DateForwarded.Date, dateFrom.Date)>=0) &&
                DateTime.Compare(x.DateForwarded.Date, dateUpto.Date) <=0))
        {
        }
    }
}