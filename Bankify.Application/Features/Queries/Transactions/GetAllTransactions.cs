using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Transactions;
using MediatR;

namespace Bankify.Application.Features.Queries.Transactions
{
    public class GetAllTransactions:IRequest<OperationalResult<List<ATransaction>>>
    {

    }
    internal class GetAllTransactionsQueryHandler:IRequestHandler<GetAllTransactions, OperationalResult<List<ATransaction>>>
    {
        private readonly IRepositoryBase<ATransaction> _transactions;
        private readonly INetworkService _networlkService;

        public GetAllTransactionsQueryHandler(IRepositoryBase<ATransaction> transactions, INetworkService networlkService)
        {
            _transactions = transactions;
            _networlkService = networlkService;
        }
        public async Task<OperationalResult<List<ATransaction>>> Handle(GetAllTransactions request, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<List<ATransaction>>();
            try 
            {
                var dbReachable = await _networlkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach database!");
                    return result;
                }

               var allTransactions=await _transactions.WhereAsync(t=>t.RecordStatus!=RecordStatus.Deleted, "TransactionEntries");
                if (allTransactions.Count == 0)
                {
                    result.AddError(ErrorCode.NotFound, "No Transaction Found");
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
