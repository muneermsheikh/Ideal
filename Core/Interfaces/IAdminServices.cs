using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.Masters;

namespace Core.Interfaces
{
    public interface IAdminServices
    {
        Task<IReadOnlyList<RequirementPendingDto>> PendingRequirements(int[] enquiryIds);
        Task<IReadOnlyList<RequirementPendingDto>> PendingRequirements();
        Task<IReadOnlyList<CVForward>> CVForwardDetailsOfDLs(int[] enquiryIds);
        Task<List<SelStatsDto>> SelStatsOfEnquiry (int EnquiryId);
    }
}