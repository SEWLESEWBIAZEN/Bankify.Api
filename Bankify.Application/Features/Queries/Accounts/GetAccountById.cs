using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Shared;
using MediatR;

namespace Bankify.Application.Features.Queries.Accounts
{
    public class GetAccountById:IRequest<OperationalResult<Account>>
    {
        public int Id { get; set; }
    }
    internal class GetAccountByIdQueryHandler:IRequestHandler<GetAccountById, OperationalResult<Account>>
    {
        private readonly IRepositoryBase<Account> _accounts;
        private readonly INetworkService _networkService;
        public GetAccountByIdQueryHandler(IRepositoryBase<Account> accounts, INetworkService networkService)
        {
            _accounts = accounts;
            _networkService = networkService;
        }

        public async Task<OperationalResult<Account>> Handle(GetAccountById request, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<Account>();
            try
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach to database)");
                    return result;
                }
                if (request.Id == 0 || request.Id==null) 
                {
                    result.AddError(ErrorCode.EmptyRquest, "Empty Id is Sent");
                    return result;
                }
                var account=await _accounts.FirstOrDefaultAsync(a=>a.Id==request.Id && a.RecordStatus==RecordStatus.Active, "AccountType", "User");
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
