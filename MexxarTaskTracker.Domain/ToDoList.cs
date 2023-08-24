using System.ComponentModel.DataAnnotations.Schema;

namespace MexxarTaskTracker.Domain
{
    public class ToDoList : BaseEntity
    {
        public long UserId { get; set; }
        public string? ToDoListName { get; set; }
        public string? ToDoListDescription { get; set; }
        public ICollection<Task> Tasks { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
