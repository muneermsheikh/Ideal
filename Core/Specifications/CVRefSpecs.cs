using System;
using System.Linq;
using System.Linq.Expressions;
using Core.Entities.Admin;
using Core.Enumerations;

namespace Core.Specifications
{
    public class CVRefSpecs : BaseSpecification<CVRef>
    {
        

        public CVRefSpecs(): base (x => x.RefStatus==enumSelectionResult.Referred)
        {
            AddOrderBy(x => x.EnquiryId);
            AddOrderBy(x => x.EnquiryItem);
        }

        public CVRefSpecs(string dummy, int[] enquiryItemIds): base (x => enquiryItemIds.Contains(x.EnquiryItemId))
        {
        }

        public CVRefSpecs(int[] enquiryIds): base (x => enquiryIds.Contains(x.EnquiryId))
        {
        }
        public CVRefSpecs(DateTime date1, DateTime date2)
            : base (x =>                     
                (DateTime.Compare(x.DateForwarded.Date, date1.Date) >= 0 &&
                DateTime.Compare(x.DateForwarded.Date, date2.Date) <= 0))
        {
            AddOrderBy(x => x.DateForwarded.Date);
        }


        public CVRefSpecs(CVRefParam param)
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
            
            ApplyPaging(param.PageSize * (param.PageIndex-1), param.PageSize);

                if (!string.IsNullOrEmpty(param.Sort))
                {
                    switch (param.Sort.ToLower())
                    {
                        case "enquiryid":
                            AddOrderBy(x => x.EnquiryId);
                            break;
                        case "enquiryiddesc":
                            AddOrderByDescending(x => x.EnquiryId);
                            break;

                        case "dateforwardeddesc":
                            AddOrderByDescending(x => x.DateForwarded.Date);
                            break;
                        default:
                            AddOrderBy(x => x.DateForwarded.Date);
                            break;
                }
            }
        }
    }
}