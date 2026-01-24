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
    public class UpdateTransactionType : IRequest<OperationalResult<TransactionType>>
    {
        public UpdateTransactionTypeRequest UpdateTransactionTypeRequest { get; set; }
    }

    internal class UpdateTransactionTypeCommandHandler : IRequestHandler<UpdateTransactionType, OperationalResult<TransactionType>>
    {
        private readonly IRepositoryBase<TransactionType> _transactionTypes;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IActionLoggerService _actionLoggerService;

        private ISession session;

        public UpdateTransactionTypeCommandHandler(IRepositoryBase<TransactionType> transactionTypes, INetworkService networkService, IHttpContextAccessor contextAccessor, IActionLoggerService actionLoggerService)
        {
            _transactionTypes = transactionTypes;
            _networkService = networkService;
            _contextAccessor = contextAccessor;

            session = _contextAccessor.HttpContext.Session;
            _actionLoggerService = actionLoggerService;
        }

        public async Task<OperationalResult<TransactionType>> Handle(UpdateTransactionType updateTransactionType, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<TransactionType>();
            var sessionUser = session.GetString("user");
            var request = updateTransactionType.UpdateTransactionTypeRequest;
            try
            {
                var debReachable = await _networkService.IsConnected();
                if (!debReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach Database");
                    return result;
                }
                if (request.Id == null)
                {
                    result.AddError(ErrorCode.EmptyRquest, "Incomplete Request, Empty Id Sent");
                    return result;
                }
                var transactionTypeToBeUpdated = await _transactionTypes.FirstOrDefaultAsync(tt => tt.Id == request.Id);
                if (transactionTypeToBeUpdated==null)
                {
                    result.AddError(ErrorCode.NotFound, $"Transaction Type namely: {request.Name} is not Found");
                    return result;
                }
              
                transactionTypeToBeUpdated.Name = request.Name;
                transactionTypeToBeUpdated.Description = request.Description;
                transactionTypeToBeUpdated.UpdateAudit(sessionUser);

                await _transactionTypes.UpdateAsync(transactionTypeToBeUpdated);
                await _actionLoggerService.TakeActionLog(ActionType.Update, "Transaction Type", transactionTypeToBeUpdated.Id, sessionUser, $"Transaction Type namely: {transactionTypeToBeUpdated.Name} was updated at {DateTime.Now} by {sessionUser}");
                result.Message = "Transaction Type Updated";
                result.Payload = transactionTypeToBeUpdated;
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
    }
}
