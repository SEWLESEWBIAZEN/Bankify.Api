
using AutoMapper;
using Bankify.Application.Common.DTOs.Accounts.Response;
using Bankify.Application.Common.DTOs.AccountTypes.Response;
using Bankify.Application.Common.DTOs.ActionLogs.Response;
using Bankify.Application.Common.DTOs.Auth.Response;
using Bankify.Application.Common.DTOs.TransactionEntries.Response;
using Bankify.Application.Common.DTOs.Transactions.Response;
using Bankify.Application.Common.DTOs.Transfers.Response;
using Bankify.Application.Common.DTOs.Users.Response;
using Bankify.Domain.Models;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Transactions;
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

            //transactions
            CreateMap<ATransaction, TransactionDetail>();         

            //transfers
            CreateMap<Transfer, TransferDetail>();

            //Authorization
            CreateMap<AppRole, AppRoleDetail>();
            CreateMap<AppClaim, AppClaimDetail>();
            CreateMap<RoleClaim, RoleClaimDetail>();          
            CreateMap<UserRole, UserRoleDetail>();

            //transaction entry
            CreateMap<Account,MinimalAccountDetail>();
            CreateMap<TransactionEntry, TransactionEntryDetail>();

            //login response
            CreateMap<LoginResponse, LoginResponseDetail>();
            
        }      
       

    }
}
