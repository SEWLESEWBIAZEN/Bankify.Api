using Bankify.Domain.Models.Users;

namespace Bankify.Application.Common.DTOs.Users.Response
{
    public class AppRoleDetail
    {
        public int Id { get; set; }
        public string RoleName { get; set; }    =String.Empty;   
        public ICollection<RoleClaimDetail>? RoleClaims { get; set; }
    }
}
