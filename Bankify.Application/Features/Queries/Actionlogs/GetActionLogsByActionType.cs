using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models;
using Bankify.Domain.Models.Shared;
using MediatR;

namespace Bankify.Application.Features.Queries.Actionlogs
{
    public class GetActionLogsByActionType : IRequest<OperationalResult<List<ActionLog>>>
    {
        public ActionType ActionType { get; set; } = new ActionType();

    }

    internal class GetActionLogsByActionTypeQueryHandler : IRequestHandler<GetActionLogsByActionType, OperationalResult<List<ActionLog>>>
    {
        private readonly IRepositoryBase<ActionLog> _actionLogs;
        private readonly INetworkService _networkService;
        public GetActionLogsByActionTypeQueryHandler(IRepositoryBase<ActionLog> actionLogs, INetworkService networkService)
        {
            _actionLogs = actionLogs;
            _networkService = networkService;
        }

        public async Task<OperationalResult<List<ActionLog>>> Handle(GetActionLogsByActionType request, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<List<ActionLog>>();
            try
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Unable to reach database");
                    return result;
                }

                var actionLogs = await _actionLogs.WhereAsync(al => al.Action == request.ActionType);
                if (actionLogs.Count == 0)
                {
                    result.AddError(ErrorCode.NotFound, "No Action Log Found!");
                    return result;
                }
                result.Payload = actionLogs;
                return result;
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
                return result;
            }

        }
    }


}