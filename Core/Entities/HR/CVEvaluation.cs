using System;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;

namespace Core.Entities.HR
{
    public class CVEvaluation: BaseEntity
    {
        public CVEvaluation()
        {
        }

        public CVEvaluation(int enquiryItemId, int candidateId, string fullName, 
            int applicationNo, int hRExecutiveId, int? hRSupervisorId, int? hRManagerId)
        {
            CandidateId = candidateId;
            FullName = fullName;
            ApplicationNo = applicationNo;
            EnquiryItemId = enquiryItemId;
            HRExecutiveId = hRExecutiveId;
            HRSupervisorId = hRSupervisorId;
            HRManagerId = hRManagerId;
        }

        public int CandidateId { get; set; }
        public string FullName {get; set; }
        public int ApplicationNo {get; set; }
        public int EnquiryItemId { get; set; }
        public int HRExecutiveId { get; set; }
        public DateTimeOffset SubmittedByHRExecOn { get; set; }=DateTimeOffset.Now;
        public bool? ReviewedByHRSup {get; set; }
        public int? HRSupervisorId { get; set; }
        public enumItemReviewStatus? HRSupReviewResult { get; set; }
        public DateTimeOffset? ReviewedByHRSupOn { get; set; }
        public int? HRManagerId { get; set; }
        public bool? ReviewedByHRM {get; set; }
        public DateTime? ReviewedByHRMOn { get; set; }
        public enumItemReviewStatus? HRMReviewResult { get; set; }
        public int CVReferredId { get; set; }
        public Candidate Candidate{get; set; }
        public EnquiryItem EnquiryItem {get; set; }
    }
}