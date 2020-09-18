using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.HR;
using Core.Entities.Masters;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IAssessmentService
    {
        
        Task<Assessment> CreateAssessment(int enquiryItemId, int candidateId, string loggedInUserName);
        Task<Assessment> UpdateAssessment(Assessment assessment);
        Task<IReadOnlyList<Assessment>> GetAssessmentBySpec(AssessmentParam assessmentParam);
        Task<int> DeleteAssessment(Assessment assessment);

//Q bank
        Task<IReadOnlyList<AssessmentQBank>> GetQBankForACategory(int categoryId);
        Task<IReadOnlyList<AssessmentQBank>> GetQBank_All();
        Task<IReadOnlyList<AssessmentQBank>> AddQListToAssessmentQBank (IReadOnlyList<AssessmentQBank> Qs);
        Task<int> UpdateAssessmentQsBank(List<AssessmentQBank> Qs);
        Task<int> deleteAssessmentQsBank(List<AssessmentQBank> Qs);

//Enquiry Item Q
        Task<IReadOnlyList<AssessmentQ>> CopyStddQToAssessmentQOfEnquiryItem(int id);
        Task<IReadOnlyList<AssessmentQ>> CopyQToAssessmentQOfEnquiryItem(int id);
        Task<IReadOnlyList<AssessmentQ>> GetAssessmentQsOfEnquiryItem (int enquiryItemId);
        Task<int> UpdateQsOfEnquiryItem(List<AssessmentQ> assessmentQs);

//domain subjects
/*
        Task<IReadOnlyList<DomainSub>> AddDomains(IReadOnlyList<clsString> domainList);
        Task<int> DeleteDomains(List<DomainSub> domainSubs);
        Task<int> UpdateDomains(List<DomainSub> domainList);
        Task<IReadOnlyList<DomainSub>> GetDomainList();
*/
    }
}