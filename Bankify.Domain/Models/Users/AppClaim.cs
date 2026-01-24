using Bankify.Domain.Common.Shared;

namespace Bankify.Domain.Models.Users
{
    public class AppClaim 
    {
        public int Id { get; set; }       
        public string ClaimString { get; set; }
        public ICollection<RoleClaim> RoleClaims { get; set; }
    }
}
