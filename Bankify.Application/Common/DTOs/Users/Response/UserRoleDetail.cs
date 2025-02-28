namespace Bankify.Application.Common.DTOs.Users.Response
{
    public class UserRoleDetail
    {
        public int Id { get; set; }
        public int AppRoleId { get; set; }
        public AppRoleDetail AppRole { get; set; }
        public int AppUserId { get; set; }

    }
}
