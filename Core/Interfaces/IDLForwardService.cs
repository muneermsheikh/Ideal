using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IDLForwardService
    {
        Task<DLForwarded> DLForwardToHRAsync(DateTime dtForwarded, 
            IReadOnlyList<IdInt> enquiryIdList, int iHRManagerId);

        Task<IReadOnlyList<EnquiryForwarded>> DLForwardToAssociatesAsync (
            IReadOnlyList<IdInt>  officialsList, int enquiryId, 
            IReadOnlyList<IdInt> enquiryItemIds, string mode, DateTime dtForwarded);
            
        Task<int> DeleteEnquiryItemForwardedByIdAsync (EnquiryForwarded enquiryForwarded);
        Task<int> UpdateEnquiryItemForwardedAsync (EnquiryForwarded enquiryForwarded);
        
        Task<IReadOnlyList<EnquiryForwarded>> GetEnquiriesForwardedForAnEnquiry (EnqForwardSpecParams enqFwdParams);
    }
}