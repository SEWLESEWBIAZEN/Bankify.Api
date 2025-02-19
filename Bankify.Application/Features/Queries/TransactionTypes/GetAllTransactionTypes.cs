using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Transactions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Bankify.Application.Features.Queries.TransactionTypes
{
    public class GetAllTransactionTypes:IRequest<OperationalResult<List<TransactionType>>>
    {

    }
    internal class GetAllTransactionTypesQueryHandler:IRequestHandler<GetAllTransactionTypes, OperationalResult<List<TransactionType>>>
    {
        private readonly IRepositoryBase<TransactionType> _transactionTypes;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _contextAccessor;

        private ISession session;

        public GetAllTransactionTypesQueryHandler( INetworkService networkService, IHttpContextAccessor contextAccessor, IRepositoryBase<TransactionType> transactionTypes)
        {
            
            _networkService = networkService;
            _contextAccessor = contextAccessor;
            session = _contextAccessor.HttpContext.Session;
            _transactionTypes = transactionTypes;
        }

        public async Task<OperationalResult<List<TransactionType>>> Handle(GetAllTransactionTypes request, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<List<TransactionType>>();
            var sessionUser = session.GetString("user");
            try 
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable) 
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error (Unable reach to Database");
                    return result;
                }
                var transactionTypes = await _transactionTypes.WhereAsync(tt => tt.RecordStatus != RecordStatus.Deleted);
                if (transactionTypes.Count == 0)
                {
                    result.AddError(ErrorCode.NotFound, "No Transaction Type Found");
                    return result;
                }
                result.Payload = transactionTypes;
            }
            catch (Exception ex) 
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }


    }
}
