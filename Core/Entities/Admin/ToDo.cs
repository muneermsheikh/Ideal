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

        public ToDo(int ownerId, int assignedToId, DateTimeOffset taskDate, DateTimeOffset completeBy, 
            string taskDescription,  enumTaskType? taskType, int? enquiryId, int? enquiryItemId)
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

        public int? EnquiryId {get; set; }
        public int? EnquiryItemId { get; set; }      // FK - EnquiryItem.Id
        public Employee Owner { get; set; }            // FK - Employee.Id
        public int OwnerId { get; set; }
        public Employee AssignedTo { get; set; }       // FK - Employee.Id
        public int AssignedToId { get; set; }   
        public DateTimeOffset TaskDate { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset CompleteBy { get; set; }
        public enumTaskType? TaskType { get; set; } = enumTaskType.Administrative;
        public string TaskDescription { get; set; }
        public enumTaskStatus TaskStatus { get; set; }=enumTaskStatus.NotStarted;
        public virtual List<TaskItem> TaskItems { get; set; }
    }
}