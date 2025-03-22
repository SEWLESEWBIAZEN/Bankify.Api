namespace Bankify.Application.Common.DTOs.Users.Response
{
    public class UserRolesDetails
    {
        public int Id { get; set; }       
        public List<AppRoleDetail> AppRoles { get; set; }=new List<AppRoleDetail>();
        public int AppUserId { get; set; }


    }
}
