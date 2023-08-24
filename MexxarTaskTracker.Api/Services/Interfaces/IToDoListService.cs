using MexxarTaskTracker.Domain;

namespace MexxarTaskTracker.Api.Services.Interfaces
{
    public interface IToDoListService
    {
        object GetAllToDoLists(int offset, int limit);
        object GetToDoListById(long toDoListId);
        object GetToDoListByUser(long userId);
        object CreateUpdateToDoList(ToDoListDto toDoListDto);
        bool DeleteToDoList(long toDoListId);
    }
}
