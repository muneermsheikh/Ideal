using System;
using System.Collections.Generic;
using Core.Entities.EnquiryAggregate;

namespace API.Dtos
{
    public class EnquiryItemToReturnDto
    {
        public int Id {get; set; }
        public int CategoryItemId { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public bool ECNR { get; set; }
        public DateTime CompleteBy { get; set; }
        public string Status {get; set;}
        public string AssessingSup {get; set;}
        public string AssessingHRM {get; set;}

        public IReadOnlyList<JobDescDto> JobDescs {get; set; }
        public IReadOnlyList<RemunerationDto> Remunerations {get; set; }
    }
}