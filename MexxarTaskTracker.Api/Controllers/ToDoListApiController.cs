using MexxarTaskTracker.Api.Services.Interfaces;
using MexxarTaskTracker.Domain;
using MexxarTaskTracker.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace MexxarTaskTracker.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoListApiController : ControllerBase
    {
        private readonly IToDoListService _toDoListService;
        private readonly ResponseDto _response;

        public ToDoListApiController(IToDoListService toDoListService)
        {
            _toDoListService = toDoListService;
            _response = new ResponseDto();
        }

        [HttpGet]
        [Route("GetAllToDoLists/{offset}/{limit}")]
        public object GetAllToDoListByUsers(int offset, int limit)
        {
            try
            {
                var result = _toDoListService.GetAllToDoLists(offset, limit);

                _response.Result = result;

                _response.DisplayMessage = result == null ? "" : "Get All ToDoLists";
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
        [Route("GetToDoListById/{toDoListId}")]
        public object GetToDoListById(int toDoListId)
        {
            try
            {
                var result = _toDoListService.GetToDoListById(toDoListId);

                _response.Result = result;

                _response.DisplayMessage = result == null ? "" : "Get ToDoLists by toDoListId - " + toDoListId + " ";
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
        [Route("GetToDoListByUser/{userId}")]
        public object GetToDoListByUser(string userId)
        {
            try
            {
                var result = _toDoListService.GetToDoListByUser(userId);

                _response.Result = result;

                _response.DisplayMessage = result == null ? "" : "Get ToDoLists by userId - " + userId + " ";
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
        [Route("AddToDoList")]
        public object CreateTask([FromBody] ToDoListDto toDoListDto)
        {
            try
            {
                var result = _toDoListService.CreateUpdateToDoList(toDoListDto);

                _response.Result = result;
                _response.DisplayMessage = result == null ? "ToDoList is exist" : "ToDoList is added";
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
        [Route("UpdateToDoList")]
        public object UpdateToDoList([FromBody] ToDoListDto toDoListDto)
        {
            try
            {
                var result = _toDoListService.CreateUpdateToDoList(toDoListDto);

                _response.Result = result;
                _response.DisplayMessage = "ToDoList is updated";
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
        [Route("DeleteToDoList/{toDoListId}")]
        public object DeleteToDoList(int toDoListId)
        {
            try
            {
                var result = _toDoListService.DeleteToDoList(toDoListId);

                _response.Result = result;
                _response.DisplayMessage = result ? "ToDoList is removed" : "Empty ToDoList";
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