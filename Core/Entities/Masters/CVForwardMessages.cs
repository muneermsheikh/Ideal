using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Masters
{
    [NotMapped]
    public class CVForwardMessages
    {
        public DateTime DateForwarded {get; set; }
        public DateTime EnquiryDate {get; set; }   
        public int EnquiryNo {get; set; }
        public string CategoryRef {get; set; }
        public bool includePhoto {get; set; }
        public bool includeSalary {get; set; }
        public bool includeGrade {get; set; }
        public string SenderName {get; set;}
        public string SenderDesignation {get; set;}
        public string SenderPhone {get; set;}
        public List<CVForwardCandidate> candidates {get; set; }

    }
}