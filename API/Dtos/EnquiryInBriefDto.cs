using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class EnquiryInBriefDto
    {
        public int Id {get; set;}
        public DateTime EnquiryDate {get; set; }
        public CustomerInBriefDto Customer {get; set; }
        public string ProjectManager {get; set; }
        public IReadOnlyList<EnquiryItemInBriefDto> EnquiryItems {get; set; }
        public string Remarks {get; set; }
    }
}