using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Enumerations;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface ITaskService
    {
// tOdO
        Task<ToDo> CreateTaskAsync (ToDo todo);
        Task<IReadOnlyList<ToDo>> GetTasksAsync(TaskSpecParams taskSpecParams);
        Task<ToDo> GetTaskAsync(int enquiryItemId, string taskType, 
            string taskStatus, bool onlyHeaders, int assignedToId);
        Task<ToDo> GetTaskEnquiryitemIdAssignedToIdTaskTypeAsync(
            int enquiryItemId, int assignedToId, string taskType);
            
        Task<IReadOnlyList<ToDo>> GetTaskListAsync(string taskType, 
            bool onlyHeaders, int userId);
        Task<IReadOnlyList<ToDo>> GetTaskListAsync (int enquiryItemId, string taskType, 
            string taskStatus, int ownerId, bool onlyHeaders);
        
        Task<IReadOnlyList<ToDo>> GetOwnerTaskListAsync(int ownerId, string taskStatus);
        Task<IReadOnlyList<ToDo>> GetAssignedToTaskListAsync(int assignedToId, string taskStatus);
        
        Task<ToDo> UpdateTaskAsync(ToDo toDo);
        Task<bool> DeleteTaskAsync(ToDo toDo);
        Task<bool> DeleteTaskByIdAsync(int taskId);
        Task<ToDo> UpdateTaskStatus(int TaskId, string taskStatus, DateTime statusDate, string remarks);

// TaskItem   
        Task<TaskItem> AppendTaskItemAsync(int taskId, TaskItem taskItem);
        Task<TaskItem> UpdateTaskItemAsync(TaskItem taskItem);
        Task<bool> DeleteTaskItemAsync(TaskItem taskItem);
    }
}