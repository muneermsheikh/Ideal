using System;
using System.Collections.Generic;
using Core.Entities.Admin;
using Core.Entities.Masters;
using Core.Enumerations;


namespace Core.Entities.EnquiryAggregate
{
    public class Enquiry : BaseEntity
    {
        public Enquiry()
        {
        }

        public Enquiry(IReadOnlyList<Entities.EnquiryAggregate.EnquiryItem> enquiryItems, string buyerEmail)
        {
            EnquiryItems = enquiryItems;
            BuyerEmail = buyerEmail;
        }

        public int EnquiryNo { get; set; }
        public DateTimeOffset EnquiryDate { get; set; } = DateTimeOffset.Now;
        public Address ShipToAddress { get; set; }
        public IReadOnlyList<EnquiryItem> EnquiryItems { get; set; }
        public enumEnquiryStatus EnquiryStatus { get; set; } = enumEnquiryStatus.Pending;
        public Employee ProjectManager { get; set; }
        public CustomerOfficial AccountExecutive { get; set; }
        public CustomerOfficial HRExecutive { get; set; }
        public CustomerOfficial LogisticsExecutive { get; set; }
        public string EnquiryRef { get; set; }
        public DateTime CompleteBy { get; set; }
        public Employee ReviewedBy { get; set; }
        public DateTime ReviewedOn { get; set; }
        public string Remarks { get; set; }
        public string BuyerEmail { get; set; }
        
    }
}