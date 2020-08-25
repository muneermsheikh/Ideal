using System;
using Core.Enumerations;

namespace API.Dtos
{
    public class CVEvaluationToAddDto
    {
        public int CandidateId { get; set; }
        public int EnquiryItemId { get; set; }
        public int HRExecutiveId { get; set; }
        public DateTime SubmittedByHRExecOn { get; set; }=DateTime.Now;
        public enumItemReviewStatus HRSupReviewResult { get; set; }
        

    }
}