using AutoMapper;
using MexxarTaskTracker.Api.Services.Interfaces;
using MexxarTaskTracker.Domain;
using MexxarTaskTracker.Infrastructure.UnitOfWork;

namespace MexxarTaskTracker.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly IGenericUnitOfWork _genericUnitOfWork;
        private readonly IMapper _mapper;

        public TaskService(IGenericUnitOfWork genericUnitOfWork,
            IMapper mapper)
        {
            _genericUnitOfWork = genericUnitOfWork;
            _mapper = mapper;
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
