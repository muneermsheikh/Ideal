using System;
using System.Collections.Generic;
using Core.Entities.Admin;

namespace API.Dtos
{
    public class EnqToForwardDto
    {
        //public DateTime dtForwarded {get; set;}
        public int enquiryId {get; set; }
        //public string sMode {get; set; }
        //public IReadOnlyList<CustomerOfficial> officialListDto {get; set;}
        //public EnqHeaderToForwardDto enqToForwardDto {get; set; }
        public IReadOnlyList<int> EnqItemsIds { get; set; }
    }

    
}