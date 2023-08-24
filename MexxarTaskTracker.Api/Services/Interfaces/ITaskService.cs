using MexxarTaskTracker.Domain;

namespace MexxarTaskTracker.Api.Services.Interfaces
{
    public interface ITaskService
    {
        object GetAllTasks(int offset, int limit);
        UserTaskDto GetTaskById(long taskId);
        object GetTaskByToDoListId(long toDoListId);
        object CreateUpdateTask(UserTaskDto userTaskDto);
        bool DeleteTask(long taskId);
    }
}
