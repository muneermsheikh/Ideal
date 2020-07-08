using System;
using System.Collections.Generic;
using Core.Entities.Identity;

namespace API.Dtos
{
    public class EnquiryToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public Address ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public int ShippingPrice { get; set; }
        public IReadOnlyList<EnquiryItemDto> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
    }
}