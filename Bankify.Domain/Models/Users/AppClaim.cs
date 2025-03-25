

namespace Bankify.Domain.Models.Users
{
    public class AppClaim 
    {
        public int Id { get; set; }       
        public string ClaimString { get; set; }=String.Empty;
        public ICollection<RoleClaim> RoleClaims { get; set; }=new List<RoleClaim>();
    }
}
