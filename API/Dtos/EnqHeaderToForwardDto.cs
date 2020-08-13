using System;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class EnqHeaderToForwardDto
    {
        [Required]
        public int CustomerId {get; set; }
        [Required]
        public string CustomerName {get; set; }
        [Required]
        public string EmploymentCity { get; set; }
        [Required]
        public int EnquiryNo { get; set; }
        [Required]
        public DateTime EnquiryDate { get; set; } 
    }

}