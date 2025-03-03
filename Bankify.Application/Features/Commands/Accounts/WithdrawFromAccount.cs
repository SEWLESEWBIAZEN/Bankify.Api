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
        private readonly IRepositoryBase<ATransaction> _transactions;
        private readonly IRepositoryBase<TransactionType> _transactionTypes;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IActionLoggerService _actionLoggerService;

        private ISession session;

        public WithdrawFromAccountCommandhandler(IRepositoryBase<Account> accounts, IRepositoryBase<ATransaction> transactions, INetworkService networkService, IHttpContextAccessor contextAccessor, IActionLoggerService actionLoggerService, IRepositoryBase<TransactionType> transactionTypes)
        {
            _accounts = accounts;
            _transactions = transactions;
            _networkService = networkService;
            _contextAccessor = contextAccessor;

            session = _contextAccessor.HttpContext.Session;
            _actionLoggerService = actionLoggerService;
            _transactionTypes = transactionTypes;
        }

        public async Task<OperationalResult<ATransaction>> Handle(WithdrawFromAccount WithdrawFromAccountrequest, CancellationToken cancellationToken)
        {
            var sessionUser = session.GetString("user");
            var result = new OperationalResult<ATransaction>();
            var request = WithdrawFromAccountrequest.WithdrawFromAccountRequest;
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

                //if not found, return not found error
                if (accountToBeDebited == null)
                {
                    result.AddError(ErrorCode.NotFound, "Account doesn't exist or not active");
                    return result;
                }

                //if selected and existed,check and update the balance
                if (request.Ammount +100 >= accountToBeDebited.Balance)
                {
                    result.AddError(ErrorCode.UnknownError, "The Account has no sufficient balance to withdraw");
                    return result;
                }
                var previousBalance= accountToBeDebited.Balance;
                accountToBeDebited.Balance = accountToBeDebited.Balance - request.Ammount;

                //update the entry as a whole to save changes made to the account
                var transactionSuccess= await _accounts.UpdateAsync(accountToBeDebited);

                //register the action to action logger service
                await _actionLoggerService.TakeActionLog(ActionType.Withdraw, "Account", accountToBeDebited.Id, sessionUser, $"Account with AccNo: '{accountToBeDebited.AccountNumber}' was debited '{accountToBeDebited.CurrencyCode}{request.Ammount} at {DateTime.Now} by '{sessionUser}' ");

                //since it is a transaction, it should be registered as transaction
                var newTransaction = new ATransaction
                {
                    Reason = "Withdrawal",
                    TransactionTypeId = 2,
                    BalanceBeforeTransaction = previousBalance,
                    BalanceAfterTransaction = accountToBeDebited.Balance,
                    AccountId = accountToBeDebited.Id,
                    Status = TransactionStatus.Pending,
                    TransactionDate = DateTime.Now
                };
                transactionSuccess = await _transactions.AddAsync(newTransaction);
                //retrieve transaction type object for log purpose
                var transactionType = await _transactionTypes.FirstOrDefaultAsync(tt => tt.Id == newTransaction.TransactionTypeId);
                
                //log the create transaction action
                await _actionLoggerService.TakeActionLog(ActionType.Create, "Transaction", newTransaction.Id, sessionUser, $"New Transaction was Created at {newTransaction.TransactionDate} with type of '{transactionType.Name}' and status of '{newTransaction.Status}'");
                var transactionStatusTobeUpdated = await _transactions.FirstOrDefaultAsync(t => t.Id == newTransaction.Id);
                //if transaction succeed change transaction status from pending to completed
                if (transactionSuccess)
                {
                    transactionStatusTobeUpdated.Status = TransactionStatus.Completed;
                    await _actionLoggerService.TakeActionLog(ActionType.ChangeStatus, "Transaction", transactionStatusTobeUpdated.Id, "System Auto", $"Transaction with Id: '{transactionStatusTobeUpdated.Id}' was 'completed'  at {DateTime.Now}");
                }
                //otherwise change from pending to failed. And register either change status actions to action log
                else
                {
                    transactionStatusTobeUpdated.Status = TransactionStatus.Failed;
                    await _actionLoggerService.TakeActionLog(ActionType.ChangeStatus, "Transaction", transactionStatusTobeUpdated.Id, "System Auto", $"Transaction with Id: '{transactionStatusTobeUpdated.Id}' was 'Failed'  at {DateTime.Now}");
                }
                //save changes to db
                var updateStatusSuccess = await _transactions.UpdateAsync(transactionStatusTobeUpdated);

                if (updateStatusSuccess)
                {
                    result.Message = $"Account: {accountToBeDebited.AccountNumber} was  Debited 'ETB{request.Ammount}'  Successfully";
                }
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
    }


}
