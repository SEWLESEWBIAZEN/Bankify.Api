using Bankify.Application.Common.DTOs.Accounts.Request;
using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Transactions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Bankify.Application.Features.Commands.Accounts
{
    public class DepositToAccount:IRequest<OperationalResult<ATransaction>>
    {
        public DepositToAccountRequest DepositToAccountRequest { get; set; }
    }

    internal class DepositToAccountCommandhandler:IRequestHandler<DepositToAccount, OperationalResult<ATransaction>>
    {
        private readonly IRepositoryBase<Account> _accounts;
        private readonly IRepositoryBase<ATransaction> _transactions;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _contextAccessor;
        
        private ISession session;

        public DepositToAccountCommandhandler(IRepositoryBase<Account> accounts, IRepositoryBase<ATransaction> transactions, INetworkService networkService, IHttpContextAccessor contextAccessor)
        {
            _accounts = accounts;
            _transactions = transactions;
            _networkService = networkService;
            _contextAccessor = contextAccessor;

            session=_contextAccessor.HttpContext.Session;
        }

        public async Task<OperationalResult<ATransaction>> Handle(DepositToAccount depositToAccountrequest, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<ATransaction>();
            var request = depositToAccountrequest.DepositToAccountRequest;
            try 
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Database is not reachable");
                    return result;
                }
                if(request.AccountNumber==null || request.Ammount == 0 || request.Ammount==null)
                {
                    result.AddError(ErrorCode.ValidationError, "InComplete Request! ");
                    return result;

                }
                var accountToBeCredited = await _accounts.FirstOrDefaultAsync(ac => ac.AccountNumber == request.AccountNumber && ac.RecordStatus == RecordStatus.Active);
                if (accountToBeCredited == null)
                {
                    result.AddError(ErrorCode.NotFound, "Account doesn't exist or not active");
                    return result;
                }
                accountToBeCredited.Balance += request.Ammount;
                await _accounts.UpdateAsync(accountToBeCredited);

                var newTransaction = new ATransaction
                {


                };
                result.Message=$"Account: {accountToBeCredited.AccountNumber}  Credited {request.Ammount}{accountToBeCredited.CurrencyCode} Successfully";


            }
            catch(Exception ex) 
            {

            }
            return result;
        }
    }


}
