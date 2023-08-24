using MexxarTaskTracker.Api.Services.Interfaces;
using MexxarTaskTracker.Domain;
using MexxarTaskTracker.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace MexxarTaskTracker.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskApiController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ResponseDto _response;

        public TaskApiController(ITaskService taskService)
        {
            _taskService = taskService;
            _response = new ResponseDto();
        }

        [HttpGet]
        [Route("GetAllTasks/{offset}/{limit}")]
        public object GetAllTasks(int offset, int limit)
        {
            try
            {
                var result = _taskService.GetAllTasks(offset, limit);

                _response.Result = result;

                _response.DisplayMessage = result == null ? "No Tasks" : "Get All Tasks";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error";
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet]
        [Route("GetTaskById/{taskId}")]
        public object GetTaskById(int taskId)
        {
            try
            {
                var result = _taskService.GetTaskById(taskId);

                _response.Result = result;

                _response.DisplayMessage = result == null ? "No task" : "Get task by taskId - " + taskId + " ";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error";
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet]
        [Route("GetTaskByToDoListId/{toDoListId}")]
        public object GetTaskByToDoListId(int toDoListId)
        {
            try
            {
                var result = _taskService.GetTaskByToDoListId(toDoListId);

                _response.Result = result;

                _response.DisplayMessage = result == null ? "No task" : "Get task by toDoListId - " + toDoListId + " ";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error";
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [Route("AddTask")]
        public object CreateTask([FromBody] UserTaskDto taskDto)
        {
            try
            {
                var result = _taskService.CreateUpdateTask(taskDto);

                _response.Result = result;
                _response.DisplayMessage = result == null ? "Task is exist" : "Task is added";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error";
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPut]
        [Route("UpdateTask")]
        public object UpdateTask([FromBody] UserTaskDto taskDto)
        {
            try
            {
                var result = _taskService.CreateUpdateTask(taskDto);

                _response.Result = result;
                _response.DisplayMessage = "Task is updated";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error";
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete]
        [Route("DeleteTask/{taskId}")]
        public object DeleteTask(int taskId)
        {
            try
            {
                var result = _taskService.DeleteTask(taskId);

                _response.Result = result;
                _response.DisplayMessage = result ? "Task is removed" : "Empty Task";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Result = false;
                _response.DisplayMessage = "Error";
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

    }
}