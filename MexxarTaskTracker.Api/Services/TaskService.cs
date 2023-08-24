using AutoMapper;
using MexxarTaskTracker.Api.Services.Interfaces;
using MexxarTaskTracker.Domain;
using MexxarTaskTracker.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.SignalR;

namespace MexxarTaskTracker.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly IGenericUnitOfWork _genericUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IToDoListService _toDoListService;

        public TaskService(IGenericUnitOfWork genericUnitOfWork,
            IMapper mapper, IHubContext<NotificationHub> hubContext, IToDoListService toDoListService)
        {
            _genericUnitOfWork = genericUnitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
            _toDoListService = toDoListService;
        }

        public async Task SendTaskReminder(long taskId, long userId)
        {
            var existingTask = GetTaskById(taskId);
            if (existingTask != null)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("TaskReminder", "This is your new Task = " + existingTask.TaskName + "");
            }
        }

        public object GetAllTasks(int offset, int limit)
        {
            var allTasks = _genericUnitOfWork.TaskRepository.Get()
                .Skip(offset).Take(limit).ToList();
            return _mapper.Map<List<UserTaskDto>>(allTasks);
        }

        public UserTaskDto GetTaskById(long taskId)
        {
            var taskDetails = _genericUnitOfWork.TaskRepository.Get(t => t.Id == taskId)
                .FirstOrDefault();
            return _mapper.Map<UserTaskDto>(taskDetails);
        }

        public object GetTaskByToDoListId(long toDoListId)
        {
            var toDoListTaskDetails = _genericUnitOfWork.TaskRepository.Get(t => t.ToDoListId == toDoListId)
                .FirstOrDefault();
            return _mapper.Map<UserTaskDto>(toDoListTaskDetails);
        }

        public object CreateUpdateTask(UserTaskDto userTaskDto)
        {
            var userTask = _mapper.Map<UserTask>(userTaskDto);

            if (userTask.Id > 0)
            {
                userTask.TaskStatus = userTaskDto.TaskStatus;
                //_genericUnitOfWork.TaskRepository.Update(userTask);
            }
            else
            {
                var existingTask = GetTaskById(userTaskDto.Id);
                if (existingTask == null)
                {
                    var toDoList = (ToDoListDto)_toDoListService.GetToDoListById(existingTask.ToDoListId);
                    if (toDoList != null)
                    {
                        SendTaskReminder(toDoList.Id, toDoList.UserId);
                        _genericUnitOfWork.TaskRepository.Insert(userTask);
                    }
                }
            }
            _genericUnitOfWork.Commit();
            return _mapper.Map<UserTaskDto>(userTask);
        }

        public bool DeleteTask(long taskId)
        {
            try
            {
                var existingTask = GetTaskById(taskId);
                if (existingTask != null)
                {
                    existingTask.IsDeleted = true;
                    //_genericUnitOfWork.TaskRepository.Delete(existingTask);
                    _genericUnitOfWork.Commit();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }
    }
}
