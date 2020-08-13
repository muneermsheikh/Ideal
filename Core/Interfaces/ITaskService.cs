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
        Task<ToDo> GetTaskEnquiryitemIdAssignedToIdTaskTypeAsync(
            int enquiryItemId, int assignedToId, enumTaskType taskType);
        Task<IReadOnlyList<ToDo>> GetTaskListAsync(enumTaskType taskType, 
            bool onlyHeaders, int userId);
        Task<IReadOnlyList<ToDo>> GetTaskListAsync (int enquiryItemId, enumTaskType taskType, 
            enumTaskStatus taskStatus, bool onlyHeaders, int ownerId);
        
        Task<IReadOnlyList<ToDo>> GetOwnerTaskListAsync(int ownerId, enumTaskStatus taskStatus);
        Task<IReadOnlyList<ToDo>> GetAssignedToTaskListAsync(int assignedToId, enumTaskStatus taskStatus);
        
        Task<ToDo> UpdateTaskAsync(ToDo toDo);
        Task<bool> DeleteTaskAsync(ToDo toDo);
        Task<bool> DeleteTaskByIdAsync(int taskId);

// TaskItem        
        Task<TaskItem> AppendTaskItemAsync(int taskId, TaskItem taskItem);
        Task<TaskItem> UpdateTaskItemAsync(TaskItem taskItem);
        Task<bool> DeleteTaskItemAsync(TaskItem taskItem);
    }
}