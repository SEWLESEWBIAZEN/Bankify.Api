using Bankify.Domain.Common.Shared;

namespace Bankify.Domain.Models.Users
{
    public class AppRole:BaseEntity
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RoleClaim> RoleClaims { get; set; }
      
    }
}
