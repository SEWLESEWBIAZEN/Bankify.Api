using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Transactions;
using MediatR;

namespace Bankify.Application.Features.Queries.Transactions
{
    public class GetTransactionsByStatus : IRequest<OperationalResult<List<ATransaction>>>
    {
        public TransactionStatus TransactionStatus { get; set; }

    }
    internal class GetTransactionsByStatusQueryHandler : IRequestHandler<GetTransactionsByStatus, OperationalResult<List<ATransaction>>>
    {
        private readonly IRepositoryBase<ATransaction> _transactions;        
        private readonly INetworkService _networkService;

        public GetTransactionsByStatusQueryHandler(IRepositoryBase<ATransaction> transactions, INetworkService networkService)
        {
            _transactions = transactions;
            _networkService = networkService;
          
        }
        public async Task<OperationalResult<List<ATransaction>>> Handle(GetTransactionsByStatus request, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<List<ATransaction>>();
            try
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach database!");
                    return result;
                }
              
                var allTransactions = await _transactions.WhereAsync(t => t.RecordStatus != RecordStatus.Deleted && t.Status ==request.TransactionStatus,"TransactionEntries");
                if (allTransactions.Count == 0)
                {
                    result.AddError(ErrorCode.NotFound, $" No {request.TransactionStatus.ToString()} Transaction Found");
                    return result;
                }
                result.Payload = allTransactions.OrderByDescending(t => t.TransactionDate).ToList();
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
    }
}
