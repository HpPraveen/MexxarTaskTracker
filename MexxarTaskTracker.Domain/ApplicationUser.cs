using MexxarTaskTracker.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace MexxarTaskTracker.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string? UserName { get; set; }
        public Gender Gender { get; set; }
        public string? Email { get; set; }
        public DateTime? SysCreatedOn { get; set; }
    }
}
