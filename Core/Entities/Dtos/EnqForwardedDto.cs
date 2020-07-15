using System;

namespace Core.Entities.Dtos
{
    public class EnqForwardedDto
    {
        public int EnquiryId { get; set; }
        public int CustomerId { get; set; }
        public int CustomerOfficialId { get; set; }
        public int EnquiryItemId { get; set; }
        public DateTimeOffset ForwardedOn { get; set; } = DateTimeOffset.Now;

    }
}