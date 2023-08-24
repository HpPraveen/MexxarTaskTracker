namespace MexxarTaskTracker.Domain
{
    public class ToDoListDto : BaseEntityDto
    {
        public long UserId { get; set; }
        public string? ToDoListName { get; set; }
        public string? ToDoListDescription { get; set; }
        public ICollection<TaskDto> Tasks { get; set; }
    }
}
