namespace Bankify.Application.Common.DTOs.Users.Response
{
    public class UserDetail
    {
        public int Id { get; set; }
        public string FirstName { get; set; }=String.Empty;
        public string LastName { get; set; }=String.Empty;
        public string Email { get; set; }=String.Empty;
        //public string Password { get; set; }
        public string PhoneNumber { get; set; }=String.Empty;
        public string? Address { get; set; }
        public string? ProfilePicture { get; set; }
        public ICollection<UserRoleDetail> UserRoles { get; set; }=new HashSet<UserRoleDetail>();
        //public IEnumerable<Account> Accounts { get; set; }
    }
}
