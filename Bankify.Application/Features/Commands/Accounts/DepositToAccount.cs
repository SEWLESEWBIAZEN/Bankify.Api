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
        private readonly IRepositoryBase<TransactionType> _transactionTypes;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IActionLoggerService _actionLoggerService;
        
        private ISession session;

        public DepositToAccountCommandhandler(IRepositoryBase<Account> accounts, IRepositoryBase<ATransaction> transactions, INetworkService networkService, IHttpContextAccessor contextAccessor, IActionLoggerService actionLoggerService, IRepositoryBase<TransactionType> transactionTypes)
        {
            _accounts = accounts;
            _transactions = transactions;
            _networkService = networkService;
            _contextAccessor = contextAccessor;

            session = _contextAccessor.HttpContext.Session;
            _actionLoggerService = actionLoggerService;
            _transactionTypes = transactionTypes;
        }

        public async Task<OperationalResult<ATransaction>> Handle(DepositToAccount depositToAccountrequest, CancellationToken cancellationToken)
        {
            var sessionUser = session.GetString("user");
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
                var previousBalance= accountToBeCredited.Balance;
                accountToBeCredited.Balance = accountToBeCredited.Balance+ request.Ammount;
                await _accounts.UpdateAsync(accountToBeCredited);
                await _actionLoggerService.TakeActionLog(ActionType.Deposit, "Account", accountToBeCredited.Id, sessionUser, $"Account with AccNo: '{accountToBeCredited.AccountNumber}' was credited '{accountToBeCredited.CurrencyCode}{request.Ammount} at {DateTime.Now} by '{sessionUser}' ");

                var newTransaction = new ATransaction
                {
                    Reason="Saving",
                    TransactionTypeId=1,
                    BalanceBeforeTransaction = previousBalance,
                    BalanceAfterTransaction = accountToBeCredited.Balance,
                    AccountId =accountToBeCredited.Id,                   
                    Status =TransactionStatus.Pending,
                    TransactionDate=DateTime.Now
                };
                newTransaction.Register(sessionUser);
                var transactionSuccess = await _transactions.AddAsync(newTransaction);
                var transactionType = await _transactionTypes.FirstOrDefaultAsync(tt => tt.Id == newTransaction.TransactionTypeId);
                await _actionLoggerService.TakeActionLog(ActionType.Create, "Transaction", newTransaction.Id, sessionUser, $"New Transaction was Created at {newTransaction.TransactionDate} with type of '{transactionType.Name}' and status of '{newTransaction.Status}'");
                var transactionStatusTobeUpdated = await _transactions.FirstOrDefaultAsync(t => t.Id == newTransaction.Id);
                if (transactionSuccess)
                {
                    transactionStatusTobeUpdated.Status = TransactionStatus.Completed;
                    await _actionLoggerService.TakeActionLog(ActionType.ChangeStatus, "Transaction", transactionStatusTobeUpdated.Id, "System Auto", $"Transaction with Id: '{transactionStatusTobeUpdated.Id}' was 'completed'  at {DateTime.Now}");
                }
                else
                {
                    transactionStatusTobeUpdated.Status = TransactionStatus.Failed;
                    await _actionLoggerService.TakeActionLog(ActionType.ChangeStatus, "Transaction", transactionStatusTobeUpdated.Id, "System Auto", $"Transaction with Id: '{transactionStatusTobeUpdated.Id}' was 'Failed'  at {DateTime.Now}");
                }
                var updateStatusSuccess=await _transactions.UpdateAsync(transactionStatusTobeUpdated);

                if (updateStatusSuccess)
                {
                    result.Message = $"Account: {accountToBeCredited.AccountNumber} was  Credited 'ETB{request.Ammount}'  Successfully";
                }              


            }
            catch(Exception ex) 
            {
                result.AddError(ErrorCode.ServerError, ex.Message);

            }
            return result;
        }
    }


}
