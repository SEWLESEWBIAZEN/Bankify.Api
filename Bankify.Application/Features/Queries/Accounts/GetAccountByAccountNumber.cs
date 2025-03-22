using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Shared;
using MediatR;

namespace Bankify.Application.Features.Queries.Accounts
{
    public class GetAccountByAccountNumber : IRequest<OperationalResult<Account>>
    {
        public string AccountNumber { get; set; }=String.Empty;
    }
    internal class GetAccountByAccountNumberQueryHandler : IRequestHandler<GetAccountByAccountNumber, OperationalResult<Account>>
    {
        private readonly IRepositoryBase<Account> _accounts;
        private readonly INetworkService _networkService;
        public GetAccountByAccountNumberQueryHandler(IRepositoryBase<Account> accounts, INetworkService networkService)
        {
            _accounts = accounts;
            _networkService = networkService;
        }

        public async Task<OperationalResult<Account>> Handle(GetAccountByAccountNumber request, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<Account>();
            try
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach to database)");
                    return result;
                }
                if (request.AccountNumber is null)
                {
                    result.AddError(ErrorCode.EmptyRquest, "Empty AccountNumber is Sent");
                    return result;
                }
                var account = await _accounts.FirstOrDefaultAsync(a => a.AccountNumber == request.AccountNumber && a.RecordStatus == RecordStatus.Active, "AccountType", "User");
                if (account == null)
                {
                    result.AddError(ErrorCode.NotFound, "Account Not Exist or NOT Active!");
                    return result;
                }
                result.Payload = account;

            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
    }
}
