namespace MexxarTaskTracker.Domain
{
    public class UserTaskDto : BaseEntityDto
    {
        public long ToDoListId { get; set; }
        public string? TaskName { get; set; }
        public string? TaskDescription { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
        public TaskStatus TaskStatus { get; set; }
    }
}
