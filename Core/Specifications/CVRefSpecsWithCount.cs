using System;
using System.Linq;
using System.Linq.Expressions;
using Core.Entities.Admin;
using Core.Entities.Masters;
using Core.Enumerations;

namespace Core.Specifications
{
    public class CVRefSpecsWithCount : BaseSpecification<CVRef>
    {
       
        public CVRefSpecsWithCount(): base (x => x.RefStatus==enumSelectionResult.Referred)
        {
        }

       public CVRefSpecsWithCount(string dummy, int[] enquiryItemIds): base (x => enquiryItemIds.Contains(x.EnquiryItemId))
        {
        }

        public CVRefSpecsWithCount(int[] enquiryIds): base (x => enquiryIds.Contains(x.EnquiryId))
        {
        }
        public CVRefSpecsWithCount(DateTime date1, DateTime date2)
            : base (x =>                     
                (DateTime.Compare(x.DateForwarded.Date, date1.Date) >= 0 &&
                DateTime.Compare(x.DateForwarded.Date, date2.Date) <= 0))
        {
            AddOrderBy(x => x.DateForwarded.Date);
        }


        public CVRefSpecsWithCount(CVRefParam param)
            :  base( x => 
            (
                (!param.Id.HasValue || x.Id == param.Id) &&
                (!param.EnquiryId.HasValue || x.EnquiryId == param.EnquiryId) &&
                (!param.EnquiryItemId.HasValue || x.EnquiryItemId == param.EnquiryItemId) &&
                (!param.ForwardedFrom.HasValue && !param.ForwardedUpto.HasValue|| 
                    (DateTime.Compare(x.DateForwarded.Date, (DateTime)param.ForwardedFrom) >= 0 &&
                    DateTime.Compare(x.DateForwarded.Date, (DateTime)param.ForwardedUpto) <= 0))
            ))
        {            
        }
    }
}