using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.Dtos;
using Core.Entities.EnquiryAggregate;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IDLForwardService
    {
        Task<IReadOnlyList<EnquiryForwarded>> CreateEnquiryForwardForEnquiryIdAsync (
            IReadOnlyList<CustOfficialToForwardDto> officialsDto, 
            int enquiryId, string mode, DateTimeOffset dtForwarded);
        Task<IReadOnlyList<EnquiryForwarded>> CreateEnquiryForwardForSelectedEnquiryItemsAsync (
            IReadOnlyList<CustOfficialToForwardDto> officialsDto, 
            IReadOnlyList<EnquiryItem> enquiryItems, string mode, DateTimeOffset dtForwarded);

        Task<int> DeleteEnquiryItemForwardedByIdAsync (EnquiryForwarded enquiryForwarded);
        Task<int> UpdateEnquiryItemForwardedAsync (EnquiryForwarded enquiryForwarded);
        
        Task<IReadOnlyList<EnquiryForwarded>> GetEnquiriesForwardedForAnEnquiry (EnqForwardSpecParams enqFwdParams);
    }
}