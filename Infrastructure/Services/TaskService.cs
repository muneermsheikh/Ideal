using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Enumerations;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class TaskService : IToDo
    {
        private readonly IUnitOfWork _unitOfWork;
        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<TaskItem> AppendTaskItemAsync(int taskId, DateTimeOffset transDate, string transationDetail, DateTimeOffset? remindOn, int? qntyConcluded, bool? createEmailMessage, enumTaskItemStatus? itemStatus)
        {
            throw new NotImplementedException();
        }

        public Task<ToDo> CreateATaskAsync(int ownerId, int assignedToId, DateTimeOffset taskDate, DateTimeOffset completeBy, DateTimeOffset remindOn, string taskDescription, string senderEmail, string addresseeEmail, int? enquiryId, int? enquiryItemId, enumTaskType? taskType, bool? sendMail, enumTaskStatus? taskStatus)
        {
            throw new NotImplementedException();
        }

        public Task<ToDo> DeleteTaskByIdAsync(ToDo toDo1)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteTaskItemAsync(TaskItem taskItem)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ToDo>> GetPendingTaskListOfAnOwnerAsync(int taskOwnerId)
        {
            throw new NotImplementedException();
        }

        public Task<ToDo> GetTaskByIdAsync(int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TaskItem>> GetTaskItemsByTaskIdAsync(int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ToDo>> GetTaskListWithItemsOfAnOwnerAsync(int taskOwnerId)
        {
            throw new NotImplementedException();
        }

        public Task<ToDo> UpdateTaskAsync(ToDo toDo)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateTaskItemAsync(TaskItem taskItem)
        {
            var t = await _unitOfWork.Repository<TaskItem>().UpdateAsync(taskItem);
            await _unitOfWork.Complete();
            return t;
        }
    }
}