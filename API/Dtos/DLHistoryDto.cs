using System.Collections.Generic;
using API.Dto;

// DL -> DL Category -> Candidates Referred -> Process details
namespace API.Dtos
{
    public class DLHistoryDto
    {
        public DLHistoryDto()
        {
        }

        public string EnquiryNo {get; set;}
        public string EnquiryDated {get; set;}
        public string ProjectManager {get; set;}
        public List<DLCategory> DLCategories {get; set;}
    }

    public class DLCategory
    {
        public DLCategory(string categoryRef, int qnty, List<CandidateRef> candidatesRef)
        {
            CategoryRef = categoryRef;
            Qnty = qnty;
            CandidatesRef = candidatesRef;
        }

        public string CategoryRef {get; set;}
        public int Qnty {get; set; }
        public List<CandidateRef> CandidatesRef {get; set; }
    }

    public class CandidateRef
    {
        public CandidateRef(string candidateName, string referredBy, List<HistoryProcess> candidatesProcessHistory)
        {
            CandidateName = candidateName;
            ReferredBy = referredBy;
            CandidatesProcessHistory = candidatesProcessHistory;
        }

        public string CandidateName {get; set;}
        public string ReferredBy {get; set;}
        public List<HistoryProcess> CandidatesProcessHistory {get; set;}
    }

}