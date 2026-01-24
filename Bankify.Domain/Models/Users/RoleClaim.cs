using Bankify.Domain.Common.Shared;

namespace Bankify.Domain.Models.Users
{
    public class RoleClaim : BaseEntity
    {
        public int Id { get; set; }
        public int AppClaimId {get;set;}
        public AppClaim AppClaim { get; set;}
        public int AppRoleId { get;set;}
        public  AppRole AppRole { get; set;}

    }
}
