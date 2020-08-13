using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class CustomerToReturnDto
    {
        public int Id { get; set; }
        public string CustomerType { get; set; }
        public string CustomerName { get; set; }
        public string KnownAs { get; set; }
        public string CityName { get; set; }
        public string Email {get; set; }
        public string Phone1 { get; set; }

        public List<CustomerOfficialDto> CustomerOfficials { get; set; }
        
        
    }
}