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

        public Enquiry(int customerId, int enquiryNo, int projectManagerId, string enqRef, string basketId, 
            int hrexecutiveId, int logisticsExecutiveId, IReadOnlyList<EnquiryItem> enqItems)
        {
            EnquiryNo = enquiryNo;
            CustomerId = customerId;
            ProjectManagerId = projectManagerId;
            EnquiryItems = enqItems;
            EnquiryRef = enqRef;
            BasketId = basketId;
            HRExecutiveId = hrexecutiveId;
            LogisticsExecutiveId = logisticsExecutiveId;
        }

      /*  public Enquiry(IReadOnlyList<EnquiryItem> enquiryItems, string buyerEmail)
        {
            EnquiryItems = enquiryItems;
            BuyerEmail = buyerEmail;
        }
    */
        public int CustomerId {get; set; }
        public int EnquiryNo { get; set; }
        public string BasketId {get; set;}
        public DateTime EnquiryDate { get; set; } = DateTime.Now;
        //public virtual SiteAddress ShipToAddress { get; set; }
        public IReadOnlyList<EnquiryItem> EnquiryItems { get; set; }
        public bool ReadyToReview {get; set; } = false;         // this is set to true when all enquiryitems are reviewed
        public enumEnquiryStatus EnquiryStatus { get; set; } = enumEnquiryStatus.Pending;
        public virtual Employee ProjectManager { get; set; }
        public int ProjectManagerId {get; set; }
        public Customer Customer {get; set; }
        public virtual CustomerOfficial AccountExecutive { get; set; }
        public int? AccountExecutiveId {get; set; }
        public virtual CustomerOfficial HRExecutive { get; set; }
        public int? HRExecutiveId {get; set; }
        public virtual CustomerOfficial LogisticsExecutive { get; set; }
        public int? LogisticsExecutiveId {get; set; }
        public string EnquiryRef { get; set; }
        public DateTime? CompleteBy { get; set; }
        public virtual Employee ReviewedBy { get; set; }
        public DateTime? ReviewedOn { get; set; }
        public string Remarks { get; set; }
        
    }
}