using System;
using System.Collections.Generic;
using Core.Entities.Admin;
using Core.Entities.Masters;

namespace Core.Entities.Dtos
{
    public class CustomerDtoToSave
    {
       public string CustomerType { get; set; }
        public string CustomerName { get; set; }
        public string KnownAs { get; set; }
        public string CityName { get; set; }
        public virtual List<IndustryType> IndustryTypes { get; set; }
        public string IntroducedBy { get; set; }
        public string Email {get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string CompanyUrl { get; set; }
        public string Description { get; set; }
        public string CustomerStatus { get; set; } 

        public virtual List<CustomerAddress> CustomerAddresses {get; set; }
        public int? CustomerAddressId { get; set; }
        public List<CustomerOfficial> CustomerOfficials { get; set; }
        public int CustomerOfficialId { get; set; }
        public DateTimeOffset AddedOn { get; set; } 
        public virtual Grade Grade { get; set; }
    }
}