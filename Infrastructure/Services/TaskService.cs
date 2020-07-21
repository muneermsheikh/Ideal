using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TaskItem> AppendTaskItemAsync(int taskId, TaskItem taskItem)
        {
            taskItem.TaskId = taskId;
            return  await _unitOfWork.Repository<TaskItem>().AddAsync(taskItem);
        }

        public async Task<ToDo> CreateTaskAsync(ToDo todo)
        {
            return await _unitOfWork.Repository<ToDo>().AddAsync(todo);
        }

        public async Task<bool> DeleteTaskAsync(ToDo toDo)
        {
            var result = await _unitOfWork.Repository<ToDo>().DeleteAsync(toDo);
            return result != 0 ? true : false;
        }

        public async Task<bool> DeleteTaskItemAsync(TaskItem taskItem)
        {
            var result = await _unitOfWork.Repository<TaskItem>().DeleteAsync(taskItem);
            return result != 0 ? true : false;
        }
        
        public async Task<bool> DeleteTaskByIdAsync(int taskId)
        {
            var t = await _unitOfWork.Repository<ToDo>().GetByIdAsync(taskId);
            var x = await _unitOfWork.Repository<ToDo>().DeleteAsync(t);
            return x == 0 ? false : true;
        }

        public async Task<IReadOnlyList<ToDo>> GetTaskListAsync(int enquiryItemId, 
            enumTaskType taskType, enumTaskStatus taskStatus, int userId, bool includeItems)
        {
            var spec = new TaskSpecs(enquiryItemId, taskType, enumTaskStatus.NotStarted, 
                includeItems, userId);
            return await _unitOfWork.Repository<ToDo>().GetEntityListWithSpec(spec);
        }

        public async Task<ToDo> UpdateTaskAsync(ToDo toDo)
        {
            return await _unitOfWork.Repository<ToDo>().UpdateAsync(toDo);
        }

        public async Task<TaskItem> UpdateTaskItemAsync(TaskItem taskItem)
        {
            return await _unitOfWork.Repository<TaskItem>().UpdateAsync(taskItem);
        }

        public async Task<IReadOnlyList<ToDo>> GetTasksAsync(TaskSpecParams taskSpecParams)
        {
            var sec = new TaskSpecs(taskSpecParams);
            return await _unitOfWork.Repository<ToDo>().GetEntityListWithSpec(sec);
        }

        public async Task<IReadOnlyList<ToDo>> GetTaskAsync(
            enumTaskType taskType, bool onlyHeaders, int userId)
        {
            var spec = new TaskSpecs(taskType, userId, onlyHeaders);
            return await _unitOfWork.Repository<ToDo>().GetEntityListWithSpec(spec);
        }

        public async Task<IReadOnlyList<ToDo>> GetTaskListAsync(
            enumTaskType taskType, bool onlyHeaders, int userId)
        {
            var spec = new TaskSpecs(taskType, userId, onlyHeaders);
            return await _unitOfWork.Repository<ToDo>().GetEntityListWithSpec(spec);
        }

        public async Task<IReadOnlyList<ToDo>> GetTaskListAsync(int enquiryItemId, 
            enumTaskType taskType, enumTaskStatus taskStatus, bool onlyHeaders, int ownerId)
        {
            var spec = new TaskSpecs(enquiryItemId, taskType,taskStatus,onlyHeaders,ownerId);
            return await _unitOfWork.Repository<ToDo>().GetEntityListWithSpec(spec);
        }

        public async Task<ToDo> GetTaskEnquiryitemIdAssignedToIdTaskTypeAsync(int enquiryItemId, int assignedToId, enumTaskType taskType)
        {
            var spec = new TaskSpecs(enquiryItemId,assignedToId, taskType);
            return await _unitOfWork.Repository<ToDo>().GetEntityWithSpec(spec);
        }

        public Task<IReadOnlyList<ToDo>> GetTaskListOwnersAsyncUser(int ownerId, enumTaskStatus taskStatus)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<ToDo>> GetTaskListAssignedTosAsyncUser(int assignedToId, enumTaskStatus taskStatus)
        {
            throw new System.NotImplementedException();
        }
    }
}