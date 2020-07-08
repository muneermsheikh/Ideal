using System;
using System.Collections.Generic;

namespace Core.Entities.Admin
{
    public class EnquiryForwarded: BaseEntity
    {
        public EnquiryForwarded()
        {
            ForwardedOn = DateTime.Now;
        }

        public int CustomerId { get; set; }
        public int CustomerOfficialId { get; set; }
        public int EnquiryItemId { get; set; }
        public DateTime ForwardedOn { get; set; }
        public virtual List<CVSource> CVSources { get; set; }       // to update later.
    }
}