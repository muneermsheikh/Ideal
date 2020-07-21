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

        public Enquiry(int customerId, string buyerEmail)
        {
            CustomerId = customerId;
            BuyerEmail = buyerEmail;
        }

        public Enquiry(IReadOnlyList<EnquiryItem> enquiryItems, string buyerEmail)
        {
            EnquiryItems = enquiryItems;
            BuyerEmail = buyerEmail;
        }

        public int CustomerId {get; set; }
        public int EnquiryNo { get; set; }
        public DateTimeOffset EnquiryDate { get; set; } = DateTimeOffset.Now;
        public SiteAddress ShipToAddress { get; set; }
        public IReadOnlyList<EnquiryItem> EnquiryItems { get; set; }
        // when the EnquriyStatus is approved, 
        // it triggers passing on the requirement to HR Department - model OrderForwardToHRDivn
        public bool ReadyToReview {get; set; } = false;         // this is set to true when all enquiryitems are reviewed
        public enumEnquiryStatus EnquiryStatus { get; set; } = enumEnquiryStatus.Pending;
        public Employee ProjectManager { get; set; }
        public Customer Customer {get; set; }
        public CustomerOfficial AccountExecutive { get; set; }
        public CustomerOfficial HRExecutive { get; set; }
        public CustomerOfficial LogisticsExecutive { get; set; }
        public string EnquiryRef { get; set; }
        public DateTimeOffset CompleteBy { get; set; }
        public Employee ReviewedBy { get; set; }
        public DateTimeOffset ReviewedOn { get; set; }
        public string Remarks { get; set; }
        public string BuyerEmail { get; set; }
        
    }
}