using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class TaskToReturnDto
    {
        public int Id { get; set; }
        public string AssignedTo { get; set; }    
        public string Owner {get; set; }
        public DateTimeOffset TaskDate { get; set; } 
        public DateTimeOffset CompleteBy { get; set; }
        public string TaskType { get; set; } 
        public string TaskDescription { get; set; }
        public string TaskStatus { get; set; }
        public virtual List<TaskItemToReturnDto> TaskItems { get; set; }
    }
}