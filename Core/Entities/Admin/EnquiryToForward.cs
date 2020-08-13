using System;
using System.Collections.Generic;

namespace Core.Entities.Admin
{
    public class EnquiryToForward
    {
        public int EnquiryId { get; set; }
        public List<int> EnqItemIds {get; set; }
    }
}