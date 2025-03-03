namespace Bankify.Application.Common.DTOs.Users.Response
{
    public class RoleClaimDetails
    {           
        public int AppRoleId { get; set; }
        public IEnumerable<AppClaimDetail> AppClaims { get; set; }
        
    }
}
