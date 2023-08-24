using MexxarTaskTracker.Domain.Enums;

namespace MexxarTaskTracker.Domain
{
    public class User : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender Gender { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public ICollection<ToDoList> ToDoLists { get; set; }
    }
}
