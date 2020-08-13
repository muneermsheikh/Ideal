using System;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class EnqItemForwardDto
    {
        public int Id {get; set; }
        [Required]
        public int SrNo {get; set; }
        [Required]
        public int CategoryItemId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required, Range(1,999, ErrorMessage="Quantity must be between 1 and 999")]
        public int Quantity { get; set; }
        [Required]
        public bool ECNR { get; set; }
        public int ExpDesiredInYrsMin { get; set; }
        public int ExpDesiredInYrsMax { get; set; }
        public string JobDescInBrief { get; set; }
        
        public int SalaryRangeMin { get; set; }
        public int SalaryRangeMax { get; set; }
        public int ContractPeriodInMonths { get; set; }
        public string Food { get; set; }
        public string Housing { get; set; }
        public string Transport {get; set;}
        public DateTime CompleteBy { get; set; }
    }
}