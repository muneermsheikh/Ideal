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
                (!cParam.TechnicallyFeasible.HasValue || x.TechnicallyFeasible == cParam.TechnicallyFeasible) &&
                (!cParam.CommerciallyFeasible.HasValue || x.CommerciallyFeasible == cParam.CommerciallyFeasible) &&
                (!cParam.LogisticallyFeasible.HasValue || x.LogisticallyFeasible == cParam.LogisticallyFeasible) &&
                (!cParam.VisaAvailable.HasValue || x.VisaAvailable == cParam.VisaAvailable) &&
                (!cParam.DocumentationWillBeAvailable.HasValue || 
                    x.DocumentationWillBeAvailable == cParam.DocumentationWillBeAvailable) &&
                (!cParam.HistoricalStatusAvailable.HasValue || 
                    x.HistoricalStatusAvailable == cParam.HistoricalStatusAvailable) &&
                (!cParam.SalaryOfferedFeasible.HasValue || 
                    x.SalaryOfferedFeasible == cParam.SalaryOfferedFeasible) &&
                (!cParam.HistoricalStatusAvailable.HasValue || 
                    x.HistoricalStatusAvailable == cParam.HistoricalStatusAvailable) &&
                (!cParam.Status.HasValue || x.Status == cParam.Status) &&
                (!cParam.ReviewedOn.HasValue || DateTimeOffset.Compare(
                    x.ReviewedOn.Date, (DateTimeOffset)cParam.ReviewedOn) == 0) &&
                (!cParam.ReviewedBy.HasValue || x.ReviewedBy == cParam.ReviewedBy) 
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