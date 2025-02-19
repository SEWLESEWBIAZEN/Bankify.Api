using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Transactions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Bankify.Application.Features.Queries.TransactionTypes
{
    public class GetTransactionTypeById:IRequest<OperationalResult<TransactionType>>
    {
        public int Id { get; set; }
    }
    internal class GetTransactionTypeByIdQueryHandler:IRequestHandler<GetTransactionTypeById, OperationalResult<TransactionType>>
    {
        private readonly IRepositoryBase<TransactionType> _transactionTypes;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private ISession session;

        public GetTransactionTypeByIdQueryHandler(IRepositoryBase<TransactionType> transactionTypes, INetworkService networkService, IHttpContextAccessor httpContextAccessor)
        {
            _transactionTypes = transactionTypes;
            _networkService = networkService;
            _httpContextAccessor = httpContextAccessor;

            session=_httpContextAccessor.HttpContext.Session;
        }

        public async Task<OperationalResult<TransactionType>> Handle(GetTransactionTypeById request, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<TransactionType>();
            try 
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error (Unable to reach Database)");
                    return result;
                }
                var transactionType = await _transactionTypes.FirstOrDefaultAsync(tt => tt.Id == request.Id && tt.RecordStatus != RecordStatus.Deleted);
                if(transactionType == null)
                {
                    result.AddError(ErrorCode.NotFound, "Desired Transaction Type is not found or Deleted");
                    return result;  
                }
                result.Payload = transactionType;
            }
            catch (Exception ex) 
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
    }
}
