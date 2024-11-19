using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_DTOs.Auth
{
    public class LoginDTO
    {
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        public string Password { get; set; } = string.Empty;
    }
}
