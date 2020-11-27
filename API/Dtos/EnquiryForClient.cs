using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;

namespace API.Dtos
{
    public class EnquiryForClient
    {
        public int Id {get; set; }
        public int CustomerId {get; set; }
        public string CustomerName {get; set;}
        public string CityName {get; set;}
        public string CountryName {get; set;}
        public int EnquiryNo { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string EnquiryRef { get; set; }
        public string BasketId {get; set;}
        public DateTime CompleteBy { get; set; }
        public string Assigned {get; set;}
        public string ReviewStatus {get; set;}
        
        public string EnquiryStatus { get; set; }
        
        //public IReadOnlyList<EnquiryItemForClient> enquiryItems { get; set; }
        
    }
}