using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string CustomerType { get; set; }
        public string CustomerName { get; set; }
        public string KnownAs { get; set; }
        public string CityName { get; set; }
        public string IntroducedBy { get; set; }
        public string Email {get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string CompanyUrl { get; set; }
        public string Description { get; set; }
        public string CustomerStatus { get; set; }

        public List<CustomerOfficialDto> CustomerOfficials { get; set; }
        public List<CustomerAddressDto> CustomerAddresses{get; set; }
        public DateTime AddedOn { get; set; } 
    }
}