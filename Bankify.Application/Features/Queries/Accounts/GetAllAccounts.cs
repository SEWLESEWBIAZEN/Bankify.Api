﻿using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bankify.Application.Features.Queries.Accounts
{
    public class GetAllAccounts:IRequest<OperationalResult<List<Account>>>
    {
        public RecordStatus? RecordStatus { get; set; }

    }
    internal class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccounts, OperationalResult<List<Account>>>
    {
        private readonly IRepositoryBase<Account> _accounts;
        private readonly INetworkService _networkService;
        public GetAllAccountsQueryHandler(IRepositoryBase<Account> accounts, INetworkService networkService)
        {
            _accounts = accounts;
            _networkService = networkService;
        }

        public async Task<OperationalResult<List<Account>>> Handle(GetAllAccounts request, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<List<Account>>();
            try 
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach to database)");
                    return result;
                    
                }
                var accounts = request.RecordStatus switch
                {
                    RecordStatus.Active => await _accounts.Where(a=>a.RecordStatus==RecordStatus.Active,"AccountType","User").ToListAsync(),
                    RecordStatus.InActive=>await _accounts.Where(a=>a.RecordStatus==RecordStatus.InActive,"AccountType","User").ToListAsync(),
                    _=>await _accounts.Where(a=>a.RecordStatus==RecordStatus.Active,"AccountType","User").ToListAsync(),
                };
                if (accounts.Count == 0)
                {
                    result.AddError(ErrorCode.NotFound, "No Accounts Found!");
                    return result;
                }
                result.Payload = accounts;                    
            }
            catch (Exception ex) {
            result.AddError(ErrorCode.ServerError, ex.Message);
             }
            return result;
        }
    }
}
