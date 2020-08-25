using System;
using System.Collections.Generic;

namespace Core.Entities.Admin
{
    public class EnquiryForwarded: BaseEntity
    {
        public EnquiryForwarded()
        {
        }

        public EnquiryForwarded(DateTime forwardedOn, int customerId, int customerOfficialId, 
            int enquiryId, string forwardedByMode, string addressee, 
            List<EnquiryItemForwarded> enqItemsForwarded) 
        {
            ForwardedOn = ForwardedOn;
            EnquiryId = enquiryId;
            CustomerId = customerId;
            CustomerOfficialId = customerOfficialId;
            ForwardedByMode = forwardedByMode;
            Addressee = addressee;
            EnquiryItemsForwarded = enqItemsForwarded;
        }

        public int EnquiryId { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int CustomerOfficialId { get; set; }
        public DateTime ForwardedOn { get; set; } = DateTime.Now;
        public string ForwardedByMode {get; set; }
        public string Addressee {get; set; }
        public string SentReference {get; set; }
        
        public virtual List<EnquiryItemForwarded> EnquiryItemsForwarded {get; set; }

    }
}