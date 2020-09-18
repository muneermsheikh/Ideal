using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Admin
{
    [NotMapped]
    public class CVRefToAddDto
    {
        public CVRefToAddDto()
        {
        }

        public DateTime dateForwarded {get; set;} 
        public int CustomerOfficialId {get; set; }
        public int CustomerId {get; set; }
        public string ForwardedBy {get; set;}
        public bool includePhoto {get; set; }
        public bool includeGrade {get; set;}
        public bool includeSalaryExpectation {get; set;}
        public List<CandidateAndEnqItemId> CandidateAndEnqItemIds {get; set; }
        //public virtual List<ToDo> CVRefTasks {get; set;}
    }
    
    
}