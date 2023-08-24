using System.ComponentModel.DataAnnotations;

namespace MexxarTaskTracker.Domain.DTO
{
    public class LoginDto
    {
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
