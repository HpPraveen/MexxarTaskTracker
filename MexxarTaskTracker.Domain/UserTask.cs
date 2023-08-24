using System.ComponentModel.DataAnnotations.Schema;

namespace MexxarTaskTracker.Domain
{
    public class UserTask : BaseEntity
    {
        public long ToDoListId { get; set; }
        public string? TaskName { get; set; }
        public string? TaskDescription { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
        public TaskStatus TaskStatus { get; set; }

        [ForeignKey("ToDoListId")]
        public virtual ToDoList ToDoList { get; set; }
    }
}
