using System;

namespace Core.Entities.Admin
{
    public class TaskItem: BaseEntity
    {
        public TaskItem()
        {
            TransDate = System.DateTime.Now;
            CreateEmailMessage = false;
        }

        public int TaskId { get; set; }
        public DateTime TransDate { get; set; }
        public int? QntyConcluded { get; set; }
        public string TransactionDetail { get; set; }
        public bool CreateEmailMessage { get; set; }
        public DateTime? RemindOn { get; set; }
    }
}