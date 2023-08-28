using System.ComponentModel.DataAnnotations.Schema;

namespace MexxarTaskTracker.Domain
{
    public class ToDoList : BaseEntity
    {
        public string UserId { get; set; }
        public string? ToDoListName { get; set; }
        public string? ToDoListDescription { get; set; }
        public ICollection<UserTask> Tasks { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
