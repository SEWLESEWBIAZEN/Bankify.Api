using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Accounts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bankify.Application.Features.Queries.Accounts
{
    public class GetAccountsByUserId : IRequest<OperationalResult<List<Account>>>
    {
        public int UserId { get; set; }
    }

    internal class GetAccountsByUserIdQueryHandler:IRequestHandler<GetAccountsByUserId, OperationalResult<List<Account>>>
    {
        private readonly IRepositoryBase<Account> _accounts;
        private readonly INetworkService _networkService;
        public GetAccountsByUserIdQueryHandler(IRepositoryBase<Account> accounts, INetworkService networkService)
        {
            _accounts = accounts;
            _networkService = networkService;
        }

        public async Task<OperationalResult<List<Account>>> Handle(GetAccountsByUserId request, CancellationToken cancellationToken)
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
                if (request.UserId == 0 || request?.UserId == null) 
                {
                    result.AddError(ErrorCode.EmptyRquest, "Empty User Id Sent");
                    return result;
                }

                var accountsList = await _accounts.Where(a => a.UserId == request.UserId, "AccountType", "User").ToListAsync();
                if (accountsList.Count == 0) 
                {
                    result.AddError(ErrorCode.NotFound, "No User Accounts Found!");
                    return result;
                }
                result.Payload = accountsList;
            }
            catch (Exception ex) 
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
    }
}
