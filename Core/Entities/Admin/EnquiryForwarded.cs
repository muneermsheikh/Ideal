using System;
using System.Collections.Generic;

namespace Core.Entities.Admin
{
    public class EnquiryForwarded: BaseEntity
    {
        public EnquiryForwarded()
        {
        }

        public EnquiryForwarded(DateTimeOffset forwardedOn, int customerId, int customerOfficialId, 
            int enquiryItemId, int enquiryId, string forwardedByMode, string addressee)
        {
            ForwardedOn = ForwardedOn;
            EnquiryId = enquiryId;
            CustomerId = customerId;
            CustomerOfficialId = customerOfficialId;
            EnquiryItemId = enquiryItemId;
            ForwardedByMode = forwardedByMode;
            Addressee = addressee;
        }

        public int EnquiryId { get; set; }
        public int CustomerId { get; set; }
        public int CustomerOfficialId { get; set; }
        public int EnquiryItemId { get; set; }
        public DateTimeOffset ForwardedOn { get; set; } = DateTimeOffset.Now;
        public string ForwardedByMode {get; set; }
        public string Addressee {get; set; }
        public string SentReference {get; set; }
        public Customer Customer { get; set; }
        public virtual List<CVSource> CVSources { get; set; }       // to update later.
    }
}