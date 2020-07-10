using System;
using System.ComponentModel.DataAnnotations;
using Core.Enumerations;

namespace API.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategorytName { get; set; }
        public bool ECNR { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
        public int ExpDesiredInYrsMin { get; set; }
        public int ExpDesiredInYrsMax { get; set; }
        public string JobDescInBrief { get; set; }
        public string JobDescAttachment { get; set; }
        public int SalaryRangeMin { get; set; }
        public int SalaryRangeMax { get; set; }
        public int ContractPeriodInMonths { get; set; }
        public enumProvision Food { get; set; }
        public enumProvision Housing { get; set; }
        public enumProvision Transport { get; set; }
        public DateTime DateRequiredBy { get; set; }
    }
}