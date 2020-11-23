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
        private readonly IGenericRepository<ToDo> _taskRepo;
        private readonly IGenericRepository<TaskItem> _taskItemRepo;
        public TaskService(IGenericRepository<ToDo> taskRepo, 
            IGenericRepository<TaskItem> taskItemRepo)
        {
            _taskItemRepo = taskItemRepo;
            _taskRepo = taskRepo;
        }

        public async Task<TaskItem> AppendTaskItemAsync(int taskId, TaskItem taskItem)
        {
            taskItem.TaskId = taskId;
            return await _taskItemRepo.AddAsync(taskItem);
        }

        public async Task<ToDo> CreateTaskAsync(ToDo todo)
        {
            return await _taskRepo.AddAsync(todo);
        }

        public async Task<bool> DeleteTaskAsync(ToDo toDo)
        {
            var result = await _taskRepo.DeleteAsync(toDo);
            return result != 0 ? true : false;
        }

        public async Task<bool> DeleteTaskItemAsync(TaskItem taskItem)
        {
            var result = await _taskItemRepo.DeleteAsync(taskItem);
            return result != 0 ? true : false;
        }

        public async Task<bool> DeleteTaskByIdAsync(int taskId)
        {
            var t = await _taskRepo.GetByIdAsync(taskId);
            var x = await _taskRepo.DeleteAsync(t);
            return x == 0 ? false : true;
        }

        public async Task<IReadOnlyList<ToDo>> GetTaskListAsync(int enquiryItemId,
            string taskType, string taskStatus, int userId, bool includeItems)
        {
            return await _taskRepo.GetEntityListWithSpec(
                new TaskSpecs(enquiryItemId, taskType, "NotStarted",
                includeItems, userId));
        }

        public async Task<ToDo> UpdateTaskAsync(ToDo toDo)
        {
            return await _taskRepo.UpdateAsync(toDo);
        }

        public async Task<TaskItem> UpdateTaskItemAsync(TaskItem taskItem)
        {
            return await _taskItemRepo.UpdateAsync(taskItem);
        }

        public async Task<IReadOnlyList<ToDo>> GetTasksAsync(TaskSpecParams taskSpecParams)
        {
            return await _taskRepo.GetEntityListWithSpec(new TaskSpecs(taskSpecParams));
        }

        public async Task<IReadOnlyList<ToDo>> GetTaskAsync(
            string taskType, bool onlyHeaders, int userId)
        {
            return await _taskRepo.GetEntityListWithSpec(
                new TaskSpecs(taskType, userId, onlyHeaders));
        }
        public async Task<ToDo> GetTaskAsync(int enquiryItemId, string taskType, 
            string taskStatus, bool onlyHeaders, int assignedToId)
        {
            return await _taskRepo.GetEntityWithSpec(new TaskSpecs(
                enquiryItemId, assignedToId, taskType, taskStatus));
        }
        
        public async Task<IReadOnlyList<ToDo>> GetTaskListAsync(
            string taskType, bool onlyHeaders, int userId)
        {
            return await _taskRepo.GetEntityListWithSpec(
                new TaskSpecs(taskType, userId, onlyHeaders));
        }

        public async Task<IReadOnlyList<ToDo>> GetTaskListAsync(int enquiryItemId,
            string taskType, string taskStatus, bool onlyHeaders, int ownerId)
        {
            return await _taskRepo.GetEntityListWithSpec(
                new TaskSpecs(enquiryItemId, taskType, taskStatus, onlyHeaders, ownerId));
        }

        public async Task<ToDo> GetTaskEnquiryitemIdAssignedToIdTaskTypeAsync(
            int enquiryItemId, int assignedToId, string taskType)
        {
            return await _taskRepo.GetEntityWithSpec(
                new TaskSpecs(enquiryItemId, assignedToId, taskType));
        }

        public async Task<IReadOnlyList<ToDo>> GetOwnerTaskListAsync(
            int ownerId, string taskStatus)
        {
            return await _taskRepo.GetEntityListWithSpec(new TaskSpecs(ownerId, taskStatus));
        }

        public async Task<IReadOnlyList<ToDo>> GetAssignedToTaskListAsync(
            int assignedToId, string taskStatus)
        {
            return await _taskRepo.GetEntityListWithSpec(new TaskSpecs(taskStatus, assignedToId));
        }


    }
}