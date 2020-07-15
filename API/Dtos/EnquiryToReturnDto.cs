using System;
using System.Collections.Generic;
using Core.Entities.Identity;
using Core.Enumerations;

namespace API.Dtos
{
    public class EnquiryToReturnDto
    {
        public int Id { get; set; }
        public int EnquiryNo {get; set; }
        public DateTimeOffset EnquiryDate { get; set; }
        public Address ShipToAddress { get; set; }
        public IReadOnlyList<EnquiryItemDto> EnquiryItems { get; set; }
        public string EnquiryStatus { get; set; }
        public string ProjectManager { get; set; }
        public string AccountExecutive { get; set; }
        public string HRExecutive { get; set; }
        public string LogisticsExecutive { get; set; }
        public string EnquiryRef { get; set; }
        public DateTimeOffset CompleteBy { get; set; }
        public string ReviewedBy { get; set; }
        public DateTimeOffset ReviewedOn { get; set; }
        public string Remarks { get; set; }
        public string BuyerEmail { get; set; }
    }
}
