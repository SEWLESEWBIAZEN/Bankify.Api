using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Transactions;
using MediatR;

namespace Bankify.Application.Features.Queries.TransactionEntries
{
    public class GetTransactionEntriesByTransaction : IRequest<OperationalResult<List<TransactionEntry>>>
    {
        public int TransactionId { get; set; }

    }
    internal class GetTransactionEntriesByTransactionQueryHandler : IRequestHandler<GetTransactionEntriesByTransaction, OperationalResult<List<TransactionEntry>>>
    {
        private readonly IRepositoryBase<TransactionEntry> _transactionEntries;
        private readonly INetworkService _networlkService;

        public GetTransactionEntriesByTransactionQueryHandler(IRepositoryBase<TransactionEntry> transactionEntries, INetworkService networlkService)
        {
            _transactionEntries = transactionEntries;
            _networlkService = networlkService;
        }
        public async Task<OperationalResult<List<TransactionEntry>>> Handle(GetTransactionEntriesByTransaction request, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<List<TransactionEntry>>();
            var transactionId=request.TransactionId;
            try
            {
                var dbReachable = await _networlkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach database!");
                    return result;
                }

                var allTransactionEntries = await _transactionEntries.WhereAsync(t => t.RecordStatus != RecordStatus.Deleted && t.Transaction.Id==transactionId, "Transaction", "Account");
                if (allTransactionEntries.Count == 0)
                {
                    result.AddError(ErrorCode.NotFound, "No Transaction Entry Found");
                    return result;
                }
                result.Payload = allTransactionEntries.OrderByDescending(t => t.RegisteredDate).ToList();
            }
            catch (Exception ex) 
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
    }
}

