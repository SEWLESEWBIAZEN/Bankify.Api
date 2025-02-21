namespace Bankify.Application.Common.DTOs.Users.Response
{
    public class RoleClaimDetail
    {           
        public int AppRoleId { get; set; }
        public IEnumerable<AppClaimDetail> AppClaims { get; set; }
        
    }
}
