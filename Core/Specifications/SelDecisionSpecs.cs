using System;
using System.Linq;
using System.Linq.Expressions;
using Core.Entities.Admin;
using Core.Enumerations;

namespace Core.Specifications
{
    public class SelDecisionSpecs : BaseSpecification<SelDecision>
    {
        
        public SelDecisionSpecs(int[] enquiryIds)
            : base (x => enquiryIds.Contains(x.EnquiryId))
        {
            AddOrderBy(x => x.EnquiryItemId);
        }

        public SelDecisionSpecs(string dummy, int[] enquiryItemIds)
            : base (x => enquiryItemIds.Contains(x.EnquiryItemId))
        {
            AddOrderBy(x => x.EnquiryItemId);
        }

        public SelDecisionSpecs(DateTime DateFrom, DateTime DateUpto)
            : base (x => x.SelectionDate.Date >= DateFrom.Date && x.SelectionDate.Date <= DateUpto.Date)
        {
            AddOrderBy(x => x.EnquiryId);
            AddOrderBy(x => x.EnquiryItemId);
        }

        public SelDecisionSpecs(enumSelectionResult selectionResult)
            : base (x => x.SelectionResult == selectionResult)
        {
            AddOrderBy(x => x.EnquiryId);
            AddOrderBy(x => x.EnquiryItemId);
        }
        
        public SelDecisionSpecs(SelDecisionParams param) 
        :  base( x => 
            (
                (!param.ApplicationNo.HasValue || x.ApplicationNo == param.ApplicationNo) &&
                (!param.SelDateFrom.HasValue && !param.SelDateUpto.HasValue || DateTime.Compare(
                    x.SelectionDate.Date, (DateTime)param.SelDateFrom.Value.Date) >= 0 &&
                    DateTime.Compare(x.SelectionDate, (DateTime)param.SelDateUpto.Value.Date) <=0) &&
                (!param.EnquiryId.HasValue || x.EnquiryId == param.EnquiryId) &&
                (!param.EnquiryItemId.HasValue || x.EnquiryItemId == param.EnquiryItemId) &&
                (!param.CVRefId.HasValue || x.CVRefID == param.CVRefId)
            ))
        {
            if (param.PageSize != 0)
            {
                ApplyPaging(param.PageSize, param.PageSize * (param.PageIndex-1));
                AddOrderBy(x => x.EnquiryId);
                AddOrderBy(x => x.EnquiryItemId);
            }
        }
    }
}
