using System;
using Core.Enumerations;

namespace Core.Entities.Admin
{
    public class TaskItem: BaseEntity
    {
        public TaskItem()
        {
            TransDate = System.DateTime.Now;
            CreateEmailMessage = false;
        }

        public TaskItem(int taskId, DateTime transDate, int? qntyConcluded, string transactionDetail, bool createEmailMessage, DateTime? remindOn)
        {
            TaskId = taskId;
            TransDate = transDate;
            QntyConcluded = qntyConcluded;
            TransactionDetail = transactionDetail;
            CreateEmailMessage = createEmailMessage;
            RemindOn = remindOn;
        }
        public TaskItem(DateTime transDate, int? qntyConcluded, string transactionDetail, bool createEmailMessage, DateTime? remindOn)
        {
            TransDate = transDate;
            QntyConcluded = qntyConcluded;
            TransactionDetail = transactionDetail;
            CreateEmailMessage = createEmailMessage;
            RemindOn = remindOn;
        }

        public int TaskId { get; set; }
        public DateTime TransDate { get; set; } = DateTime.Now;
        public int? QntyConcluded { get; set; }
        public string TransactionDetail { get; set; }
        public bool CreateEmailMessage { get; set; }
        public DateTime? RemindOn { get; set; }
        public string ItemStatus {get; set;}
    }
}