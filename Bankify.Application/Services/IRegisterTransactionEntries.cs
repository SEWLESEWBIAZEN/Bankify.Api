using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Transactions;
using Microsoft.AspNetCore.Http;
namespace Bankify.Application.Services
{
    public interface IRegisterTransactionEntriesService
    {
        Task<OperationalResult<List<TransactionEntry>>> RegisterTransactionEntriesAsync(List<Account> accounts, decimal amount, TransactionType transactionType);
    }
    public class RegisterTransactionEntriesService : IRegisterTransactionEntriesService
    {
        private IRepositoryBase<TransactionEntry> _transactionEntries;
        private IRepositoryBase<ATransaction> _transactions;
        private IActionLoggerService _actionLoggerService;
        private IHttpContextAccessor _contextAccessor;
        private ISession session;
        public RegisterTransactionEntriesService(IRepositoryBase<TransactionEntry> transactionEntries, IRepositoryBase<ATransaction> transactions, IActionLoggerService actionLoggerService, IHttpContextAccessor contextAccessor)
        {
            _transactionEntries = transactionEntries;
            _transactions = transactions;
            _actionLoggerService = actionLoggerService;
            _contextAccessor = contextAccessor;
            session=_contextAccessor.HttpContext.Session;
        }
        public async Task<OperationalResult<List<TransactionEntry>>> RegisterTransactionEntriesAsync(List<Account> accounts,decimal amount, TransactionType transactionType)
        {
            //accounts[0] ---user account
            //accounts[1] ---bank account
            var account = accounts[0];
            var liabilityAccount = accounts[1];
            var result= new OperationalResult<List<TransactionEntry>>();
            var sessionUser = session.GetString("user");
            //using (var dbTransaction = await _transactionEntries.BeginTransactionAsync())
            //{
                try
                {
                    // Create a new transaction
                    var transaction = new ATransaction
                    {
                        Description = transactionType.ToString(),
                        Status = TransactionStatus.Pending,
                        TransactionDate = DateTime.Now
                    };
                    transaction.Register(sessionUser);
                    var transactionSaveSuccess = await _transactions.AddAsync(transaction);
                    await _actionLoggerService.TakeActionLog(
                        ActionType.Create,
                        "Transaction",
                        transaction.Id,
                        sessionUser,
                        $"New transaction created at {transaction.TransactionDate} with status '{transaction.Status}'.");

                    // Update transaction status
                    if (transactionSaveSuccess)
                    {
                        transaction.Status = TransactionStatus.Completed;
                        await _transactions.UpdateAsync(transaction);

                        // Log transaction completion
                        await _actionLoggerService.TakeActionLog(
                            ActionType.ChangeStatus,
                            "Transaction",
                            transaction.Id,
                            "System Auto",
                            $"Transaction '{transaction.Id}' was completed at {DateTime.Now}.");

                        // Create transaction entry
                        var transactionEntry1 = new TransactionEntry
                        {
                            TransactionId = transaction.Id,
                            AccountId = account.Id,
                            BalanceBeforeTransaction = transactionType==TransactionType.Deposit? account.Balance - amount :account.Balance +amount,
                            BalanceAfterTransaction = account.Balance,
                            Amount = amount,
                            EntryType = transactionType == TransactionType.Deposit ? EntryType.Credit:EntryType.Debit
                        };
                        var transactionEntry2 = new TransactionEntry
                        {
                            TransactionId = transaction.Id,
                            AccountId = liabilityAccount.Id,
                            BalanceBeforeTransaction = transactionType == TransactionType.Deposit ? liabilityAccount.Balance - amount:liabilityAccount.Balance +amount,
                            BalanceAfterTransaction = liabilityAccount.Balance,
                            Amount = amount,
                            EntryType = transactionType == TransactionType.Deposit ? EntryType.Debit:EntryType.Credit
                        };
                    //returning both entries as result payload
                    result.Payload = [transactionEntry1, transactionEntry2];

                    var entrySuccess = await _transactionEntries.AddRangeAsync([transactionEntry1, transactionEntry2]);
                        if (!entrySuccess)
                        {
                            result.AddError(ErrorCode.UnknownError, "Failed to add transaction entry.");
                            return result;
                        }
                    }
                    else
                    {
                        transaction.Status = TransactionStatus.Failed;
                        await _transactions.UpdateAsync(transaction);

                        // Log transaction failure
                        await _actionLoggerService.TakeActionLog(
                            ActionType.ChangeStatus,
                            "Transaction",
                            transaction.Id,
                            "System Auto",
                            $"Transaction '{transaction.Id}' failed at {DateTime.Now}.");
                    }
                // await dbTransaction.CommitAsync();
               
                }
                catch (Exception ex)
                {
                   // await dbTransaction.RollbackAsync();
                    result.AddError(ErrorCode.ServerError, ex.Message);
                }
            //}
            return result;
        }
    }
}
