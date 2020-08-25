using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class EnquiryForwardedDto
    {
        public int EnquiryId { get; set; }
        public CustomerInBriefDto ForwardedToAssociate { get; set; }
        public DateTime ForwardedOn { get; set; } = DateTime.Now;
        //public string Customer { get; set; }
        public IReadOnlyList<EnquiryForwardedInBriefDto> EnquiryWithItemsDto { get; set; }

    }
}