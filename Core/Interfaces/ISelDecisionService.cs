using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.Dto;
using Core.Enumerations;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface ISelDecisionService
    {
         Task<IReadOnlyList<SelDecision>> InsertSelDecision(SelDecisionToAddDto selDecToAddDto);
         
         Task<int> UpdateSelDecisions(List<SelDecision> selDecisions);
         Task<SelDecision> UpdateSelDecision (SelDecision selDecision);
         Task<bool> DeleteSelDecision(SelDecision selDecision);

         Task<IReadOnlyList<CVRef>> GetPendingSelectionsAll();
         Task<IReadOnlyList<CVRef>> GetPendingSelectionsOfDLs(int[] enquiryIds);
         Task<IReadOnlyList<CVRef>> GetPendingSelectionsOfDLItems(int[] enquiryItemIds);
         
         Task<IReadOnlyList<SelDecision>> GetSelDecisionBetweenDates(DateTime Date1, DateTime Date2, enumSelectionResult result);
         Task<IReadOnlyList<SelDecision>> GetSelDecisionsofEnquiryIds(int[] enquiryIds);
         Task<IReadOnlyList<SelDecision>> GetSelDecisionsofEnquiryIdsWithStatus(int[] enquiryIds, enumSelectionResult result);
         Task<IReadOnlyList<SelDecision>> GetSelDecisionsOfEnquiryItemIds (int[] enquiryItemIds);
         Task<IReadOnlyList<SelDecision>> GetSelDecisionsOfEnquiryItemIdsWithStatus (int[] enquiryItemIds, enumSelectionResult result);
         Task<IReadOnlyList<SelDecision>> GetSelDecisionsWithSpecs (SelDecisionParams param);
    }
}