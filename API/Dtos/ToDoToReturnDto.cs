using System;

namespace API.Dtos
{
    public class ToDoToReturnDto
    {
        public DateTime TaskDate {get; set; }
        public string TaskType {get; set; }
        public string TaskOwnerName {get; set; }
        public string AssignedToName {get; set;}
        public string TaskDescription {get; set;}
        public DateTime CompleteBy {get; set; }
        
    }
}