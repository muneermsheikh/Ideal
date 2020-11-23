using System;
using System.Collections.Generic;
using Core.Enumerations;
using Core.Entities.Masters;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Admin
{
   public class ToDo: BaseEntity
    {
        public ToDo()
        {
            TaskDate = DateTime.Now;
        }

        public ToDo(int ownerId, int assignedToId, DateTime taskDate, DateTime completeBy, 
            string taskDescription,  string taskType, int? enquiryId, int? enquiryItemId)
        {
            EnquiryId = enquiryId;
            OwnerId = ownerId;
            AssignedToId = assignedToId;
            TaskDate = taskDate;
            CompleteBy = completeBy;
            TaskType = taskType;
            TaskDescription = taskDescription;
            EnquiryItemId = enquiryItemId;
        }

        public ToDo(int ownerId, int assignedToId, DateTime taskDate, DateTime completeBy, 
            string taskDescription,  string taskType, int? enquiryId, int? enquiryItemId,
            List<TaskItem> taskItems)
        {
            EnquiryId = enquiryId;
            OwnerId = ownerId;
            AssignedToId = assignedToId;
            TaskDate = taskDate;
            CompleteBy = completeBy;
            TaskType = taskType;
            TaskDescription = taskDescription;
            EnquiryItemId = enquiryItemId;
            TaskItems = taskItems;
        }

        public ToDo(int ownerId, int assignedToId, DateTime taskDate, DateTime completeBy, 
            string taskDescription,  string taskType, int? enquiryId)
        {
            EnquiryId = enquiryId;
            OwnerId = ownerId;
            AssignedToId = assignedToId;
            TaskDate = taskDate;
            CompleteBy = completeBy;
            TaskType = taskType;
            TaskDescription = taskDescription;
        }


        public int? EnquiryId {get; set; }
        public int? EnquiryItemId { get; set; }      // FK - EnquiryItem.Id
        public Employee Owner { get; set; }            // FK - Employee.Id
        [Required]
        public int OwnerId { get; set; }
        public Employee AssignedTo { get; set; }       // FK - Employee.Id
        [Required]
        public int AssignedToId { get; set; }   
        [Required]
        public DateTime TaskDate { get; set; } = DateTime.Now;
        public DateTime CompleteBy { get; set; }
        public string TaskType { get; set; } = "Administrative";
        [Required]
        public string TaskDescription { get; set; }
        public string TaskStatus { get; set; }="NotStarted";
        public virtual List<TaskItem> TaskItems { get; set; }
    }
}