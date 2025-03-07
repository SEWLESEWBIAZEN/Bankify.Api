using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Transactions;
using MediatR;

namespace Bankify.Application.Features.Queries.Transactions
{
    public class GetTransactionById:IRequest<OperationalResult<ATransaction>>
    {
        public int Id { get; set; }
    }

    internal class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionById, OperationalResult<ATransaction>> 
    {
        private readonly IRepositoryBase<ATransaction> _transactions;
        private readonly INetworkService _networkService;

        public GetTransactionByIdQueryHandler(IRepositoryBase<ATransaction> transactions, INetworkService networkService)
        {
            _transactions = transactions;
            _networkService = networkService;
        }

        public async Task<OperationalResult<ATransaction>> Handle(GetTransactionById query, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<ATransaction>();
            try 
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable) 
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error (unable to reach Database)");
                    return result;
                }
                var transaction=await _transactions.FirstOrDefaultAsync(t=>t.Id == query.Id, "TransactionEntries");
                if(transaction is null)
                {
                    result.AddError(ErrorCode.NotFound, "Transaction is Not Found");
                    return result;
                }
                result.Payload = transaction;
            }
            catch (Exception ex) 
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
    }
}
