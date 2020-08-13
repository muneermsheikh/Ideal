using System;

namespace API.Dtos
{
    public class EnqForwardedDto
    {
        public int EnquiryId { get; set; }
        public int CustomerId { get; set; }
        public int CustomerOfficialId { get; set; }
        public int EnquiryItemId { get; set; }
        public DateTime ForwardedOn { get; set; } = DateTime.Now;
        public string Customer { get; set; }

    }
}