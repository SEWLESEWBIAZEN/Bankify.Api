using Microsoft.AspNetCore.Http;

namespace Bankify.Application.Common.DTOs.Users.Request
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}
