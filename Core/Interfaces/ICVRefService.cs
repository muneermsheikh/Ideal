using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Enumerations;

namespace Core.Interfaces
{
    public interface ICVRefService
    {
         Task<IReadOnlyList<CVRef>> GetCVRefOfACandidate(int candidateId);
         Task<IReadOnlyList<CVRef>> ReferCVsToClient(int userId, int enquiryItemId, 
            List<int> candidateIds, DateTime dateForwarded, bool includeSalaryExpectation,
            bool includeGrade, bool includePhoto);
        Task<CVForward> ReferCVsToForward(CVForward cVForward);
         Task<IReadOnlyList<CVRef>> GetCVReferredForEnquiryItem(int enquiryItemId);
         Task<IReadOnlyList<CVRef>> GetCVReferredForEnquiryitemIdWithStatus(int enquiryItemId, enumSelectionResult result);
         Task<IReadOnlyList<CVRef>> GetCVReferredForEnquiryIdWithStatus(int enquiryId, enumSelectionResult result);
         Task<IReadOnlyList<CVRef>> GetCVReferredForEnquiryIdWithAllStatus(int enquiryId);
         
    //
        Task<CVForward> ForwardCVsToClient(CVForward cvForward);
        
    }
}