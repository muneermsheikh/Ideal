using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities.Admin;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TasksController : BaseApiController
    {
        private readonly IGenericRepository<ToDo> _taskRepo;
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;
        public TasksController(IGenericRepository<ToDo> taskRepo, IMapper mapper, ITaskService taskService)
        {
            _taskService = taskService;
            _mapper = mapper;
            _taskRepo = taskRepo;
        }

        [HttpPost("createSingleTask")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskToReturnDto>> CreateTask(ToDo todo)
        {
            var t = await _taskService.CreateTaskAsync(todo);
            if (t == null) return BadRequest(new ApiResponse(400 ,"Failed to create the task"));
            return Ok(t);
        }

        [HttpPost("createGroupTask")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<TaskToReturnDto>>> CreateGroupTask(IReadOnlyList<ToDo> listTodo)
        {
            var tasksAdded = await _taskRepo.AddListAsync(listTodo);
            if (tasksAdded == null) return BadRequest(new ApiResponse(400, "Failed to create the tasks"));
            
            _mapper.Map<IReadOnlyList<ToDo>, IReadOnlyList<TaskToReturnDto>>(tasksAdded);
            
            return Ok(tasksAdded);
        }


        [HttpPut("updateSingleTask")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskToReturnDto>> UpdateTask(ToDo todo)
        {
            var t = await _taskService.UpdateTaskAsync(todo);
            if (t == null) return BadRequest(new ApiResponse(400, "Failed to update the task"));
            _mapper.Map<ToDo, TaskToReturnDto>(t);
            return Ok(t);
        }

        [HttpPut("updateTaskStatus")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskToReturnDto>> UpdateTask(ToDo todo, enumTaskStatus taskStatus)
        {
            todo.TaskStatus = taskStatus;
            var t = await _taskService.UpdateTaskAsync(todo);
            if (t == null) return BadRequest(new ApiResponse(400, "Failed to update the task"));
            _mapper.Map<ToDo, TaskToReturnDto>(t);
            return Ok(t);
        }

        // refer TaskSpecParams for retrieval criteria
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Pagination<TaskToReturnDto>>> GetTasks(
            [FromQuery] TaskSpecParams tParams)
        {
            var spec = new TaskSpecs(tParams);
            var countSpec = new TaskSpecsWithFiltersForCountSpec(tParams);
            var totalItems = await _taskRepo.CountWithSpecAsync(countSpec);

            var tasks = await _taskRepo.ListWithSpecAsync(spec);
            if (tasks==null) return NotFound(new ApiResponse(404));

            var data = _mapper
                .Map<IReadOnlyList<ToDo>, IReadOnlyList<TaskToReturnDto>>(tasks);

            return Ok(new Pagination<TaskToReturnDto>
                (tParams.PageIndex, tParams.PageSize, totalItems, data));
        }

        
        [HttpDelete("{taskId}")]
        public async Task<bool> DeleteTask(int taskId)
        {
            return await _taskService.DeleteTaskByIdAsync(taskId);
        }

        [HttpPost("taskItem")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskItem>> AppendTaskItem(int taskId, TaskItem taskItem)
        {
            var t = await _taskService.AppendTaskItemAsync(taskId, taskItem);
            if (t==null) return BadRequest(new ApiResponse(400));
            return t;
        }

        [HttpPut("taskItem")]
        public async Task<ActionResult<TaskItem>> UpdateTaskItem(TaskItem taskItem)
        {
            return await _taskService.UpdateTaskItemAsync(taskItem);
        } 

        [HttpDelete("taskItem")]
        public async Task<bool> DeleteTaskItem (TaskItem taskItem)
        {
            return await _taskService.DeleteTaskItemAsync(taskItem);
        }

   
    }
}