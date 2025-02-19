using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Transactions;
using MediatR;

namespace Bankify.Application.Features.Queries.Transactions
{
    public class GetTransactionsByAccount : IRequest<OperationalResult<List<ATransaction>>>
    {
        public string AccountNumber { get; set; }

    }
    internal class GetTransactionsByAccountQueryHandler : IRequestHandler<GetTransactionsByAccount, OperationalResult<List<ATransaction>>>
    {
        private readonly IRepositoryBase<ATransaction> _transactions;
        private readonly IRepositoryBase<Account> _accounts;
        private readonly INetworkService _networlkService;

        public GetTransactionsByAccountQueryHandler(IRepositoryBase<ATransaction> transactions, INetworkService networlkService, IRepositoryBase<Account> accounts)
        {
            _transactions = transactions;
            _networlkService = networlkService;
            _accounts = accounts;
        }
        public async Task<OperationalResult<List<ATransaction>>> Handle(GetTransactionsByAccount request, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<List<ATransaction>>();
            try
            {
                var dbReachable = await _networlkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach database!");
                    return result;
                }
                var account=await _accounts.FirstOrDefaultAsync(a=>a.AccountNumber == request.AccountNumber);
                var allTransactions = await _transactions.WhereAsync(t => t.RecordStatus != RecordStatus.Deleted && t.AccountId == account.Id, "Account", "TransactionType");
                if (allTransactions.Count == 0)
                {
                    result.AddError(ErrorCode.NotFound, "No Transaction Found");
                    return result;
                }
                result.Payload = allTransactions.OrderByDescending(t=>t.TransactionDate).ToList();
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
    }
}
