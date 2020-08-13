using System;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;

namespace API.Dtos
{
    public class EnquiryItemToEditDto
    {
        public int Id {get; set; }
        public int EnquiryId { get; set; }
        public int SrNo {get; set; }
        public int CategoryItemId { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public bool ECNR { get; set; } = false;

        public int? HRExecutiveId {get; set; }
        public int? AssessingSupId { get; set; }
        public int? AssessingHRMId { get; set; }
        
        public JobDesc JobDesc {get; set; }
        public Remuneration Remuneration {get; set;}
        public DateTime? CompleteBy { get; set; }
        public enumItemReviewStatus Status { get; set; } 

    }
}