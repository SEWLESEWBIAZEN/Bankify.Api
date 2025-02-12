
using AutoMapper;
using Bankify.Application.Common.DTOs.Accounts.Response;
using Bankify.Application.Common.DTOs.AccountTypes.Response;
using Bankify.Application.Common.DTOs.ActionLogs.Response;
using Bankify.Application.Common.DTOs.Users.Response;
using Bankify.Domain.Models;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Users;

namespace Bankify.Api.MappingProfiles
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            //action logs
            CreateMap<ActionLog, ActionLogDetail>();


            //users
            CreateMap<BUser, UserDetail>();

            //accounts
            CreateMap<Account,AccountDetail>();
            CreateMap<AccountType,AccountTypeDetail>();
        }      
       

    }
}
