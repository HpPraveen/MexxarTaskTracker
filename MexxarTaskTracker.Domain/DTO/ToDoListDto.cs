namespace MexxarTaskTracker.Domain
{
    public class ToDoListDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string? ToDoListName { get; set; }
        public string? ToDoListDescription { get; set; }
        public ICollection<UserTaskDto> Tasks { get; set; }
    }
}
