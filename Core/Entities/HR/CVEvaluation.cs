using System;
using Core.Enumerations;

namespace Core.Entities.HR
{
    public class CVEvaluation: BaseEntity
    {
        public int CandidateId { get; set; }
        public int EnquiryItemId { get; set; }
        public int HRExecutiveId { get; set; }
        public DateTime SubmittedByHRExecOn { get; set; }
        public int? HRSupervisorId { get; set; }
        public enumItemReviewStatus? HRSupReviewResult { get; set; }
        public DateTime? ReviewedByHRSupOn { get; set; }
        public int? HRManagerId { get; set; }
        public DateTime? ReviewedByHRMOn { get; set; }
        public enumItemReviewStatus? HRMReviewResult { get; set; }
        public int CVReferredId { get; set; }
    }
}