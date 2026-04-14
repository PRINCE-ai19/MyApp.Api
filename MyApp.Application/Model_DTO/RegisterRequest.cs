using System.ComponentModel.DataAnnotations;

namespace MyApp.Application.Model_DTO
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "UsernameRequired")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "PasswordRequired")]
        public string Password { get; set; } = null!;

        public string? FullName { get; set; }

        [EmailAddress(ErrorMessage = "InvalidEmail")]
        public string? Email { get; set; }
    }
}
