using System;
using System.Collections.Generic;
using Core.Enumerations;
using Core.Entities.Masters;

namespace Core.Entities.Admin
{
   public class ToDo: BaseEntity
    {
        public ToDo()
        {
            TaskDate = DateTime.Now;
        }

        public int? EnquiryItemId { get; set; }      // FK - EnquiryItem.Id
        public Employee Owner { get; set; }            // FK - Employee.Id
        public int OwnerId { get; set; }
        public Employee AssignedTo { get; set; }       // FK - Employee.Id
        public int AssignedToId { get; set; }   
        public DateTime TaskDate { get; set; } = DateTime.Now;
        public DateTime CompleteBy { get; set; }
        public enumTaskType? TaskType { get; set; }
        public string TaskDescription { get; set; }
        public enumTaskStatus TaskStatus { get; set; }=enumTaskStatus.NotStarted;
        public virtual List<TaskItem> TaskItems { get; set; }
    }
}