using Microsoft.AspNetCore.Http;

namespace Bankify.Application.Common.DTOs.Users.Request
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; }=String.Empty;
        public string LastName { get; set; }=String.Empty;
        public string Email { get; set; }=String.Empty;
        public string? Password { get; set; }=String.Empty;
        public string PhoneNumber { get; set; }=String.Empty;
        public string? Address { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}
