using Bankify.Application.Common.DTOs.TransactionTypes.Request;
using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Transactions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Bankify.Application.Features.Commands.TransactionTypes
{
    public class CreateTransactionType:IRequest<OperationalResult<TransactionType>>
    {
        public CreateTransactionTypeRequest CreateTransactionTypeRequest { get; set; }
    }

    internal class CreateTransactionTypeCommandHandler:IRequestHandler<CreateTransactionType, OperationalResult<TransactionType>>
    {
        private readonly IRepositoryBase<TransactionType> _transactionTypes;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IActionLoggerService _actionLoggerService;

        private ISession session;

        public CreateTransactionTypeCommandHandler(IRepositoryBase<TransactionType> transactionTypes, INetworkService networkService, IHttpContextAccessor contextAccessor, IActionLoggerService actionLoggerService)
        {
            _transactionTypes = transactionTypes;
            _networkService = networkService;
            _contextAccessor = contextAccessor;

            session = _contextAccessor.HttpContext.Session;
            _actionLoggerService = actionLoggerService;
        }

        public async Task<OperationalResult<TransactionType>> Handle(CreateTransactionType createTransactionType, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<TransactionType>();
            var sessionUser = session.GetString("user");
            var request=createTransactionType.CreateTransactionTypeRequest;
            try 
            {
                var debReachable = await _networkService.IsConnected();
                if (!debReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach Database");
                    return result;
                }
                if (request.Name == null)
                {
                    result.AddError(ErrorCode.EmptyRquest, "Incomplete Request");
                    return result;
                }
                var alreadyExists = await _transactionTypes.ExistWhereAsync(tt => tt.Name == request.Name);
                if (alreadyExists) 
                {
                    result.AddError(ErrorCode.RecordExists, $"Transaction Type namely: {request.Name} is already Found");
                    return result;
                }
                var newTransactionType=new TransactionType();
                newTransactionType.Name = request.Name;
                newTransactionType.Description = request.Description;
                newTransactionType.Register(sessionUser);

                await _transactionTypes.AddAsync(newTransactionType);
                await _actionLoggerService.TakeActionLog(ActionType.Create, "Transaction Type", newTransactionType.Id, sessionUser, $"New Transaction Type namely: {newTransactionType.Name} was created at {DateTime.Now} by {sessionUser}");
                result.Message = "New Transaction Type Added";
                result.Payload = newTransactionType;
            }
            catch (Exception ex) 
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
    }
}
