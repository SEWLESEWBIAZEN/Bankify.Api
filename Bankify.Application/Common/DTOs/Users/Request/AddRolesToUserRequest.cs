namespace Bankify.Application.Common.DTOs.Users.Request
{
    public class AddRolesToUserRequest
    {
        public int UserId { get; set; }
        public ICollection<int> RoleIds { get; set; }
    }
}
