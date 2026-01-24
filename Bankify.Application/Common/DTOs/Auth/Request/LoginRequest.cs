using System.ComponentModel.DataAnnotations;

namespace Bankify.Application.Common.DTOs.Auth.Request
{
    public class LoginRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
