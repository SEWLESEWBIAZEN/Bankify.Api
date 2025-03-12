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
    public class WithdrawFromAccount : IRequest<OperationalResult<ATransaction>>
    {
        public WithdrawFromAccountRequest WithdrawFromAccountRequest { get; set; }
    }

    internal class WithdrawFromAccountCommandhandler : IRequestHandler<WithdrawFromAccount, OperationalResult<ATransaction>>
    {
        private readonly IRepositoryBase<Account> _accounts;
        private readonly IRegisterTransactionEntriesService _transactionEntriesService;           
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IActionLoggerService _actionLoggerService;
        private ISession session;

        public WithdrawFromAccountCommandhandler(IRepositoryBase<Account> accounts, INetworkService networkService, IHttpContextAccessor contextAccessor, IActionLoggerService actionLoggerService, IRegisterTransactionEntriesService transactionEntriesService)
        {
            _accounts = accounts;          
            _networkService = networkService;
            _contextAccessor = contextAccessor;
            session = _contextAccessor.HttpContext.Session;
            _actionLoggerService = actionLoggerService;
            _transactionEntriesService = transactionEntriesService;
        }

        public async Task<OperationalResult<ATransaction>> Handle(WithdrawFromAccount WithdrawFromAccountrequest, CancellationToken cancellationToken)
        {
            var sessionUser = session.GetString("user");
            var result = new OperationalResult<ATransaction>();
            var request = WithdrawFromAccountrequest.WithdrawFromAccountRequest;
            using (var transaction = await _accounts.BeginTransactionAsync())

            {

                try
                {
                    //check if database is accessible/reachable
                    var dbReachable = await _networkService.IsConnected();
                    if (!dbReachable)
                    {
                        result.AddError(ErrorCode.NetworkError, "Network Error(Database is not reachable");
                        return result;
                    }

                    //check if the request has account number and ammount
                    if (request.AccountNumber == null || request.Ammount == 0 || request.Ammount == null)
                    {
                        result.AddError(ErrorCode.ValidationError, "InComplete Request! ");
                        return result;

                    }

                    //select the account to be debited
                    var accountToBeDebited = await _accounts.FirstOrDefaultAsync(ac => ac.AccountNumber == request.AccountNumber && ac.RecordStatus == RecordStatus.Active);

                    //liability account
                    var liabilityAccount = await _accounts.FirstOrDefaultAsync(ac => ac.AccountNumber == "1000000000002" && ac.RecordStatus == RecordStatus.Active);

                    //if not found, return not found error
                    if (accountToBeDebited == null || accountToBeDebited.AccountNumber == "1000000000002" || liabilityAccount == null)
                    {
                        result.AddError(ErrorCode.NotFound, "Account doesn't exist or not active");
                        return result;
                    }

                    //if selected and existed,check and update the balance
                    if (request.Ammount + 100 >= accountToBeDebited.Balance)
                    {
                        result.AddError(ErrorCode.UnknownError, "The Account has no sufficient balance to withdraw");
                        return result;
                    }
                    var previousBalance = accountToBeDebited.Balance;

                    // Update account balance             
                    accountToBeDebited.Balance -= request.Ammount;
                    accountToBeDebited.UpdateAudit(sessionUser);

                    liabilityAccount.Balance -= request.Ammount;
                    liabilityAccount.UpdateAudit(sessionUser);

                    //update the entry as a whole to save changes made to the account
                    var transactionSuccess = await _accounts.UpdateRangeAsync([accountToBeDebited, liabilityAccount]);

                    //register the action to action logger service
                    await _actionLoggerService.TakeActionLog(ActionType.Withdraw, "Account", accountToBeDebited.Id, sessionUser, $"Account with AccNo: '{accountToBeDebited.AccountNumber}' was debited '{accountToBeDebited.CurrencyCode}{request.Ammount} at {DateTime.Now} by '{sessionUser}' ");

                    // Register Transaction Entries
                    var transactionEntriesRegisterResult = new OperationalResult<List<TransactionEntry>>();
                    if (transactionSuccess)
                    {
                        transactionEntriesRegisterResult = await _transactionEntriesService.RegisterTransactionEntriesAsync([accountToBeDebited, liabilityAccount], request.Ammount, TransactionType.Withdrawal);
                        if (transactionEntriesRegisterResult.IsError)
                        {
                            result.AddError(ErrorCode.UnknownError, "Transaction Entries is not registered, please handle it!");
                            return result;
                        }
                    }

                    //commiting transaction i.e saving changes to database
                    await transaction.CommitAsync(cancellationToken: cancellationToken);
                    result.Message = $"Account '{accountToBeDebited.AccountNumber}' was Debited with 'ETB{request.Ammount}' successfully.";
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    result.AddError(ErrorCode.ServerError, ex.Message);
                }
            }
            return result;
        }
    

    }


}
