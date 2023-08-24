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


        public TaskService(IGenericUnitOfWork genericUnitOfWork,
            IMapper mapper, IHubContext<NotificationHub> hubContext)
        {
            _genericUnitOfWork = genericUnitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
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
            throw new NotImplementedException();
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
