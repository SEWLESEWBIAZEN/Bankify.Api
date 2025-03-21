namespace Bankify.Application.Common.DTOs.Users.Request.AppRoles
{
    public class GrantPrivilegeRequest
    {
        public int AppRoleId { get; set; }
        public List<int> AppClaimsId { get; set; }
    }
}
