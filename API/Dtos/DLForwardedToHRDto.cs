using System;
using System.Collections.Generic;
using Core.Entities.EnquiryAggregate;

namespace API.Dtos
{
    public class DLForwardedToHRDto
    {
        public DLForwardedToHRDto()
        {
        }

        public int Id {get; set; }
        public int AssignedTo  { get; set; } 
        public string AssignedToName {get; set; }
        public DateTime AssignedOn { get; set; } 
        public IReadOnlyList<EnquiryForwardedInBriefDto> EnquiryWithItemsDto { get; set; }
        
    }
}