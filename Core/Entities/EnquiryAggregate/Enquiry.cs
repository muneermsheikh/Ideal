using System;
using System.Collections.Generic;
using Core.Entities.Admin;
using Core.Entities.EnquiriesAggregate;
using Core.Entities.Identity;
using Core.Entities.Masters;
using Core.Enumerations;

namespace Core.Entities.EnquiryAggregate
{
    public class Enquiry : BaseEntity
    {
        public Enquiry()
        {
        }

        public Enquiry(IReadOnlyList<Entities.EnquiryAggregate.EnquiryItem> enquiryItems, string buyerEmail, 
            Address shipToAddress, DeliveryMethod deliveryMethod, int subtotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            EnquiryItems = enquiryItems;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }

        public int EnquiryNo { get; set; }
        public DateTime EnquiryDate { get; set; } = DateTime.Now;
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
        public Address ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<EnquiryItem> EnquiryItems { get; set; }
        public int Subtotal { get; set; }
        public enumEnquiryStatus Status { get; set; } = enumEnquiryStatus.Pending;
        public string PaymentIntentId { get; set; }

        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod.Price;
        }
    }
}