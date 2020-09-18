using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Enumerations;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface ICVRefService
    {
         Task<IReadOnlyList<CVForwardItem>> GetCVRefOfACandidate(int candidateId);
         Task<CVForward> ReferCVsToClient(int userId, CVRefToAddDto cvrefdto);
         Task<IReadOnlyList<CVForward>> GetCVsForwarded(CVForwardParam cvfwdParam);

         Task<IReadOnlyList<CVForwardItem>> GetCVRefForEnquiryItem(int enquiryItemId);
         // *** implement this with CVRefSpecs document -- Task<IReadOnlyList<CVRef>> GetCVRefForEnquiryitemIdWithStatus(int enquiryItemId, enumSelectionResult result);
         // *** implement this with CVRefSpecs document -- Task<IReadOnlyList<CVRef>> GetCVRefForEnquiryIdWithStatus(int enquiryId, enumSelectionResult result);
         Task<IReadOnlyList<CVForwardItem>> GetCVRefForEnquiryIdWithAllStatus(int enquiryId);

         Task<List<CVForward>> GetCVForwards(int enquiryno);
         
    }
}