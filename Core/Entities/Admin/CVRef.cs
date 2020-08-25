using System;
using Core.Enumerations;

namespace Core.Entities.Admin
{
    public class CVRef: BaseEntity
    {
        public CVRef()
        {
        }

        public CVRef(int enquiryItemId, int candidateId, int applicationNo, int hrExecutiveId, 
        DateTime dateForwarded)
        {
            EnquiryItemId = enquiryItemId;
            CandidateId = candidateId;
            ApplicationNo = applicationNo;
            HRExecutiveId = hrExecutiveId;
            DateForwarded = dateForwarded;
        }

        public int EnquiryItemId { get; set; }
        public int CandidateId { get; set; }
        public int ApplicationNo {get; set; }
        public int HRExecutiveId {get; set; }
        public DateTime DateForwarded { get; set; }= DateTime.Now;
        public enumSelectionResult RefStatus { get; set; } = enumSelectionResult.Referred;
        public DateTime StatusDate { get; set; }
        public string SentReference {get; set;}
    }
}