using System.ComponentModel.DataAnnotations;

namespace Bankify.Application.Common.DTOs.Auth.Request
{
    public class LoginRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }=String.Empty;

        [Required]
        public string Password { get; set; }=String.Empty;

    }
}
