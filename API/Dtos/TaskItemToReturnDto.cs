using System;

namespace API.Dtos
{
    public class TaskItemToReturnDto
    {
        public int TaskId { get; set; }
        public DateTime TransDate { get; set; } 
        public int? QntyConcluded { get; set; }
        public string TransactionDetail { get; set; }
        public bool? CreateEmailMessage { get; set; }
        public DateTime? RemindOn { get; set; }
        public string ItemStatus {get; set;}
    }
}