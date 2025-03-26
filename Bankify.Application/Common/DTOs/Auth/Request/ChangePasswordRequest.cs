using System.ComponentModel.DataAnnotations;

namespace Bankify.Application.Common.DTOs.Auth.Request
{
    public class ChangePasswordRequest
    {
        [Required]
        public string OldPassword { get; set; } = String.Empty;

        [Required]
        public string NewPassword { get; set; } = String.Empty;
    }
}