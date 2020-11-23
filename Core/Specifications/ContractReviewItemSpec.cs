using System;
using System.Linq.Expressions;
using Core.Entities.Admin;

namespace Core.Specifications
{
    public class ContractReviewItemSpec : BaseSpecification<ContractReviewItem>
    {
        public ContractReviewItemSpec(ContractReviewItemParam cParam) 
            : base(x => (
                (string.IsNullOrEmpty(cParam.Search) || 
                    x.ServiceChargesInINR.ToLower().Contains(cParam.Search)) &&
                (!cParam.EnquiryId.HasValue || x.EnquiryId == cParam.EnquiryId) &&
                (!cParam.EnquiryItemId.HasValue || x.EnquiryItemId == cParam.EnquiryItemId) &&
                (x.TechnicallyFeasible == cParam.TechnicallyFeasible) &&
                (x.CommerciallyFeasible == cParam.CommerciallyFeasible) &&
                (x.LogisticallyFeasible == cParam.LogisticallyFeasible) &&
                (x.VisaAvailable == cParam.VisaAvailable) &&
                (x.DocumentationWillBeAvailable == cParam.DocumentationWillBeAvailable) &&
                (x.HistoricalStatusAvailable == cParam.HistoricalStatusAvailable) &&
                (x.SalaryOfferedFeasible == cParam.SalaryOfferedFeasible) &&
                (x.HistoricalStatusAvailable == cParam.HistoricalStatusAvailable) &&
                (x.Status == cParam.Status) &&
                (DateTime.Compare(x.ReviewedOn.Date, (DateTime)cParam.ReviewedOn) == 0) &&
                (x.ReviewedBy == cParam.ReviewedBy) 
            ))
        {
            AddOrderBy(x => x.ReviewedOn);
        }

        public ContractReviewItemSpec(string dummy, int enquiryId): base(x => x.EnquiryId == enquiryId)
        {
        }
        public ContractReviewItemSpec(int enquiryItemId): base(x => x.EnquiryItemId == enquiryItemId)
        {
        }
    }
    
}