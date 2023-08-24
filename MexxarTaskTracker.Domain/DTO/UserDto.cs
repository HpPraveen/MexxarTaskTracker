using MexxarTaskTracker.Domain.Enums;

namespace MexxarTaskTracker.Domain
{
    public class UserDto
    {
        public long Id { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender Gender { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string BearerToken { get; set; }
        public RoleType RoleType { get; set; }

        public ICollection<ToDoListDto> ToDoLists { get; set; }
    }
}
