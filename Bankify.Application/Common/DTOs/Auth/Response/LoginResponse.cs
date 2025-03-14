using Bankify.Application.Common.DTOs.Users.Response;
using Bankify.Domain.Models.Users;

namespace Bankify.Application.Common.DTOs.Auth.Response
{
    public class LoginResponse
    {
        public bool Success { get; set; } = true;
        public string Token { get; set; } = "";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }

    }

    public class LoginResponseDetail
    {       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public ICollection<UserRoleDetail> UserRoles { get; set; }
        public string Token { get; set; } = "";
        public bool Success { get; set; } = true;
       

    }
}
