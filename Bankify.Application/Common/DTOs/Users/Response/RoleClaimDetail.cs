﻿namespace Bankify.Application.Common.DTOs.Users.Response
{
    public class RoleClaimDetail
    {
        public int Id { get; set; }
        public int AppRoleId { get; set; }
        public AppClaimDetail AppClaim { get; set; }

    }
}
