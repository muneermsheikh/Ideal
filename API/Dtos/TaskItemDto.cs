using System;
using System.ComponentModel.DataAnnotations;
using Core.Enumerations;

namespace API.Dtos
{
    public class TaskItemDto
    {
        public int TaskId { get; set; }
        public DateTime TransDate { get; set; } = DateTime.Now;
        public int? QntyConcluded { get; set; }
        [Required(ErrorMessage="Transaction details cannot be blank")]
        [MaxLength(250)]
        public string TransactionDetail { get; set; }
        public bool? CreateEmailMessage { get; set; }
        public DateTime? RemindOn { get; set; }
        public enumTaskItemStatus? ItemStatus {get; set;}
    }
}