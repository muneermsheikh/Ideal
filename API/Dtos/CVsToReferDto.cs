using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class CVsToReferDto
    {
        public int enquiryItemId {get; set; }
        public DateTime dateForwarded {get; set;} 
        public bool includePhoto {get; set; }
        public bool includeGrade {get; set;}
        public bool includeSalaryExpectation {get; set;}
        public List<int> candidateIds {get; set; }
    }
}