using System;

namespace API.Dtos
{
    public class CVEvaluationDto
    {
        public int Id {get; set; }
        public string FullName {get; set; }
        public int ApplicationNo {get; set; }
        public string Category { get; set; }
        public string HRExecutive { get; set; }
        public DateTime SubmittedByHRExecOn { get; set; }
        public string HRSupervisor {get; set;}
        public bool? ReviewedByHRSup {get; set; }
        public DateTime ReviewedByHRSupOn {get; set;}
        public string HRSupReviewResult { get; set; }
        public string HRManager { get; set; }
        public bool? ReviewedByHRM {get; set; }
        public DateTime? ReviewedByHRMOn { get; set; }
        public string  HRMReviewResult { get; set; }
        
    }
}