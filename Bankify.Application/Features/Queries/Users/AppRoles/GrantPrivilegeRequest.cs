namespace Bankify.Application.Features.Queries.Users.AppRoles
{
    public class GrantPrivilegeRequest
    {
        public int AppRoleId { get; set; }
        public List<int> AppClaimsId { get; set; }
    }
}
