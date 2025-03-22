using Bankify.Application.Common.DTOs.Users.Response;
using Bankify.Domain.Models.Users;

namespace Bankify.Application.Common.DTOs.Auth.Response
{
    public class LoginResponse
    {
        public bool Success { get; set; } = true;
        public string Token { get; set; } = "";
        public string FirstName { get; set; }=String.Empty;
        public string LastName { get; set; }=String.Empty;
        public string Email { get; set; }=String.Empty;
        public string PhoneNumber { get; set; }=String.Empty;
        public string Address { get; set; }=String.Empty;
        public ICollection<UserRole> UserRoles { get; set; }=new List<UserRole>();

    }

    public class LoginResponseDetail
    {       
        public string FirstName { get; set; }=String.Empty;
        public string LastName { get; set; }=String.Empty;
        public string Email { get; set; }=String.Empty;
        public string PhoneNumber { get; set; }=String.Empty;
        public string Address { get; set; }=String.Empty;
        public ICollection<UserRoleDetail> UserRoles { get; set; } =new List<UserRoleDetail>();
        public string Token { get; set; } = String.Empty;
        public bool Success { get; set; } = true;
       

    }
}
