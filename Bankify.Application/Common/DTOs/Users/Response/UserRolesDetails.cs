namespace Bankify.Application.Common.DTOs.Users.Response
{
    public class UserRolesDetails
    {
        public int Id { get; set; }       
        public List<AppRoleDetail> AppRoles { get; set; }
        public int AppUserId { get; set; }


    }
}
