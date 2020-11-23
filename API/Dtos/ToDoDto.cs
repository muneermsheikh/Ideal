using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Admin;
using Core.Entities.Masters;
using Core.Enumerations;

namespace API.Dtos
{
    public class ToDoDto
    {
        public int? EnquiryId {get; set; }
        public int? EnquiryItemId { get; set; }      // FK - EnquiryItem.Id
        public Employee Owner { get; set; }            // FK - Employee.Id
        [Required (ErrorMessage="Task Owner not defined")]
        public int OwnerId { get; set; }
        public Employee AssignedTo { get; set; }       // FK - Employee.Id
        [Required (ErrorMessage="Task not assigned")]
        public int AssignedToId { get; set; }   
        public DateTime TaskDate { get; set; } = DateTime.Now;
        public DateTime CompleteBy { get; set; }
        public string TaskType { get; set; } = "Administrative";
        [Required (ErrorMessage="Description cannot be blank"), MaxLength(250)]
        public string TaskDescription { get; set; }
        [Required (ErrorMessage="Task status cannot be blank")]
        public string TaskStatus { get; set; }="NotStarted";
        public virtual IReadOnlyList<TaskItem> TaskItems { get; set; }
    }
}