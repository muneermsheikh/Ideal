using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;

namespace API.Dtos
{
    public class EnquiryToReturnDto
    {
        public int Id {get; set; }
        public int CustomerId {get; set; }
        public int EnquiryNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime EnquiryDate { get; set; }
        public CustomerToReturnDto  Customer {get; set; }

        public string EnquiryRef { get; set; }
        public string BasketId {get; set;}
        public DateTime CompleteBy { get; set; }
        
        public bool ReadyToReview {get; set; }
        public string EnquiryStatus { get; set; }
        
        public string Remarks { get; set; }
        public IReadOnlyList<EnquiryItemToReturnDto> enquiryItems { get; set; }
        
    }
}