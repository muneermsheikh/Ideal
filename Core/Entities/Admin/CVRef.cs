using System;
using System.Collections.Generic;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;
using Core.Entities.Processing;
using Core.Entities.HR;

namespace Core.Entities.Admin
{
    public class CVRef: BaseEntity
    {
        public CVRef()
        {
        }
        //public int CompareTo(CVRef c)
        //{
          //  return this.EnquiryItemId.CompareTo(c.EnquiryItemId);
        //}

        public CVRef(int id, int enquiryItemId, int candidateId, DateTime dateForwarded, List<Process> processTrans)
        {
            Id = id;
            EnquiryItemId = enquiryItemId;
            CandidateId = candidateId;
            DateForwarded = dateForwarded;
            ProcessTransactions = processTrans;
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
        public int EnquiryId {get; set;}
        public int CandidateId { get; set; }
        public int ApplicationNo {get; set; }
        public string grade {get; set;}
        public string photourl {get; set;}
        public string salaryexpectation {get; set; }
        public int HRExecutiveId {get; set; }
        public DateTime DateForwarded { get; set; }= DateTime.Now;
        public enumSelectionResult RefStatus { get; set; } = enumSelectionResult.Referred;
        public DateTime StatusDate { get; set; }
        public string SentReference {get; set;}

        public Candidate Candidate {get; set; }
        public EnquiryItem EnquiryItem {get; set;}
        public virtual SelDecision SelDecision {get; set; }
        public virtual List<Process> ProcessTransactions {get; set;}

    }
}