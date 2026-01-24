using Bankify.Domain.Common.Shared;

namespace Bankify.Domain.Models.Users
{
    public class UserRole : BaseEntity
    {
        public int Id { get; set; }
        public int AppRoleId { get; set; }
        public AppRole AppRole { get; set; }
        public int AppUserId { get; set; }
        public BUser AppUser { get; set; }
    }
}
