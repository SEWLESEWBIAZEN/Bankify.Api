﻿using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Users;

namespace Bankify.Application.Common.DTOs.Users.Response
{
    public class UserDetail
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public ICollection<UserRoleDetail> UserRoles { get; set; }
        //public IEnumerable<Account> Accounts { get; set; }
    }
}
