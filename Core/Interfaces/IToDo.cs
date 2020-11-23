using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Enumerations;

namespace Core.Interfaces
{
    public interface IToDo
    {
        Task<ToDo> CreateATaskAsync (int ownerId, int assignedToId, DateTime taskDate, 
            DateTime completeBy, DateTime remindOn, string taskDescription, 
            string senderEmail,string addresseeEmail, int? enquiryId, int? enquiryItemId, 
            string taskType, bool? sendMail, string taskStatus);
        
        Task<ToDo> GetTaskByIdAsync(int taskId);
        Task<IReadOnlyList<ToDo>> GetTaskListWithItemsOfAnOwnerAsync(int taskOwnerId);
        Task<IReadOnlyList<ToDo>> GetPendingTaskListOfAnOwnerAsync(int taskOwnerId);
        Task<ToDo> UpdateTaskAsync(ToDo toDo);
        Task<ToDo> DeleteTaskByIdAsync(ToDo toDo1);


        Task<TaskItem> AppendTaskItemAsync(int taskId, DateTime transDate, 
            string transationDetail, DateTime? remindOn, int? qntyConcluded, 
            bool? createEmailMessage, string itemStatus);
        Task<IReadOnlyList<TaskItem>> GetTaskItemsByTaskIdAsync(int taskId);
        Task<int> UpdateTaskItemAsync(TaskItem taskItem);
        Task<int> DeleteTaskItemAsync(TaskItem taskItem);

    }
}