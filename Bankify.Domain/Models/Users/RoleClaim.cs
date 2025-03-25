
using Bankify.Domain.Common.Shared;

namespace Bankify.Domain.Models.Users
{
    public class RoleClaim : BaseEntity
    {        
        public int Id { get; set; }
        public int AppClaimId {get;set;}
        public AppClaim? AppClaim { get; set;}
        public int AppRoleId { get;set;}
        public  AppRole? AppRole { get; set;}
        public static RoleClaim Create( int appRoleId, int appClaimId,string sessionUser, int id=0)
        {
            var roleClaim = new RoleClaim
            {
                Id=id,
                AppClaimId=appClaimId,
                AppRoleId=appRoleId,
                RegisteredBy=sessionUser,
            };
            return roleClaim;
        }

    }
}
