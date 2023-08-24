using MexxarTaskTracker.Domain.Enums;

namespace MexxarTaskTracker.Domain
{
    public class UserDto : BaseEntityDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender Gender { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public ICollection<ToDoListDto> ToDoLists { get; set; }
    }
}
