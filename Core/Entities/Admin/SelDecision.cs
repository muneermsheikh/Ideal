using System;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;

namespace Core.Entities.Admin
{
    public class SelDecision: BaseEntity
    {
        public SelDecision()
        {
        }

        public SelDecision(int cVRefID, string selectionRef, DateTime selectionDate, enumSelectionResult selectionResult)
        {
            CVRefID = cVRefID;
            SelectionRef = selectionRef;
            SelectionDate = selectionDate;
            SelectionResult = selectionResult;
        }
        public SelDecision(int cVRefID, int enquiryItemId, int enquiryId, int candidateId, int applicationNo, string selectionRef, DateTime selectionDate, enumSelectionResult selectionResult)
        {
            CVRefID = cVRefID;
            EnquiryItemId = enquiryItemId;
            EnquiryId = enquiryId;
            CandidateId = candidateId;
            ApplicationNo = applicationNo;
            SelectionRef = selectionRef;
            SelectionDate = selectionDate;
            SelectionResult = selectionResult;
        }

        public DateTime SelectionDate { get; set; }
        public int CVRefID {get; set;}
        public int EnquiryItemId { get; set; }
        public int EnquiryId {get; set;}
        public int CandidateId { get; set; }
        public int ApplicationNo {get; set; }
        public string SelectionRef { get; set; }
        public enumSelectionResult SelectionResult {get; set;}
        public string Remarks {get; set;}
        

    }
}