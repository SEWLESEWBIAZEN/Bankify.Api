using Bankify.Application.Common.DTOs.Accounts.Request;
using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Transactions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Bankify.Application.Features.Commands.Accounts
{
    public class DepositToAccount : IRequest<OperationalResult<ATransaction>>
    {
        public DepositToAccountRequest DepositToAccountRequest { get; set; }
    }

    internal class DepositToAccountCommandHandler : IRequestHandler<DepositToAccount, OperationalResult<ATransaction>>
    {
        private readonly IRepositoryBase<Account> _accounts;
        private readonly IRepositoryBase<ATransaction> _transactions;
        private readonly IRepositoryBase<TransactionEntry> _transactionEntries;
        private readonly IRegisterTransactionEntriesService _registerTransactionEntriesService;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IActionLoggerService _actionLoggerService;
        private readonly ILogger<DepositToAccountCommandHandler> _logger;

        public DepositToAccountCommandHandler(
            IRepositoryBase<Account> accounts,
            IRepositoryBase<ATransaction> transactions,
            IRepositoryBase<TransactionEntry> transactionEntries,
            INetworkService networkService,
            IHttpContextAccessor contextAccessor,
            IActionLoggerService actionLoggerService,
            ILogger<DepositToAccountCommandHandler> logger,
            IRegisterTransactionEntriesService registerTransactionEntriesService)
        {
            _accounts = accounts;
            _transactions = transactions;
            _transactionEntries = transactionEntries;
            _networkService = networkService;
            _contextAccessor = contextAccessor;
            _actionLoggerService = actionLoggerService;
            _logger = logger;
            _registerTransactionEntriesService = registerTransactionEntriesService;
        }

        public async Task<OperationalResult<ATransaction>> Handle(DepositToAccount request, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<ATransaction>();
            var sessionUser = _contextAccessor.HttpContext.Session.GetString("user");

            using (var transaction = await _accounts.BeginTransactionAsync())
            {

                try
                {
                    // Validate network connectivity
                    if (!await _networkService.IsConnected())
                    {
                        result.AddError(ErrorCode.NetworkError, "Network Error: Database is not reachable.");
                        return result;
                    }

                    // Validate request
                    if (request.DepositToAccountRequest == null || request.DepositToAccountRequest.Ammount <= 0 || string.IsNullOrEmpty(request.DepositToAccountRequest.AccountNumber))
                    {
                        result.AddError(ErrorCode.ValidationError, "Invalid request: Account number or amount is missing.");
                        return result;
                    }

                    // Fetch the user account
                    var account = await _accounts.FirstOrDefaultAsync(ac => ac.AccountNumber == request.DepositToAccountRequest.AccountNumber && ac.RecordStatus == RecordStatus.Active);
                    //Fetch the liability account
                    var liabilityAccount = await _accounts.FirstOrDefaultAsync(ac => ac.AccountNumber == "1000000000002" && ac.RecordStatus == RecordStatus.Active);
                    if (account == null ||account.AccountNumber== "1000000000002" || liabilityAccount==null)
                    {
                        result.AddError(ErrorCode.NotFound, "Account not found or inactive.");
                        return result;
                    }                                      
                        // Update account balance
                        var previousBalance = account.Balance;
                        account.Balance += request.DepositToAccountRequest.Ammount;
                        account.UpdateAudit(sessionUser);

                        liabilityAccount.Balance += request.DepositToAccountRequest.Ammount;
                        liabilityAccount.UpdateAudit(sessionUser);
                        var transactionSuccess = await _accounts.UpdateRangeAsync([account, liabilityAccount]);                                                         
                    
                    // Log the deposit action
                    await _actionLoggerService.TakeActionLog(
                        ActionType.Deposit,
                        "Account",
                        account.Id,
                        sessionUser,
                        $"Account '{account.AccountNumber}' was credited with '{account.CurrencyCode}{request.DepositToAccountRequest.Ammount}' by '{sessionUser}' at {DateTime.Now}.");

                    // Register Transaction Entries
                    var transactionEntriesRegisterResult = new OperationalResult<TransactionEntry>();
                    if (transactionSuccess)
                    {
                        transactionEntriesRegisterResult = await _registerTransactionEntriesService.RegisterTransactionEntriesAsync([account, liabilityAccount], request.DepositToAccountRequest.Ammount, TransactionType.Deposit);
                        if (transactionEntriesRegisterResult.IsError)
                        {
                            result.AddError(ErrorCode.UnknownError, "Transaction Entries is not registered, please handle it!");
                            return result;
                        }
                    }
                    //commiting transaction i.e saving changes to database
                    await transaction.CommitAsync(cancellationToken:cancellationToken);
                    result.Message = $"Account '{account.AccountNumber}' was credited with 'ETB{request.DepositToAccountRequest.Ammount}' successfully.";
                }
                catch (Exception ex)
                {
                    //rolling backing transaction i.e unsaving/aborting changes to database
                    await transaction.RollbackAsync(cancellationToken:cancellationToken);
                    _logger.LogError(ex, "An error occurred while processing the deposit request.");
                    result.AddError(ErrorCode.ServerError, ex.Message);
                }

            }
            return result;
        }
    }
}