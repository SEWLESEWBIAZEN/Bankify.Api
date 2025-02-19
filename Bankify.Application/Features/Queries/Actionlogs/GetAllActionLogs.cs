using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models;
using MediatR;

namespace Bankify.Application.Features.Queries.Actionlogs
{
    public class GetAllActionLogs : IRequest<OperationalResult<List<ActionLog>>>
    {

    }
    internal class GetAllActionLogsQueryHandler:IRequestHandler<GetAllActionLogs, OperationalResult<List<ActionLog>>>
    {
        private readonly IRepositoryBase<ActionLog> _actionLogs;
        private readonly INetworkService _networkService;

        public GetAllActionLogsQueryHandler(IRepositoryBase<ActionLog> actionLogs, INetworkService networkService)
        {
            _actionLogs = actionLogs;
            _networkService = networkService;
        }

        public async Task<OperationalResult<List<ActionLog>>> Handle(GetAllActionLogs request, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<List<ActionLog>>();
            try 
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error (Unable to reach database)");
                    return result;
                }
                var unorderedActionLogs = await _actionLogs.WhereAsync(al => al.Id != 0);
                var actionLogs=new List<ActionLog>( unorderedActionLogs.OrderByDescending(ual=> ual.ActionTakenOn));
                if (actionLogs.Count==0) 
                {
                    result.AddError(ErrorCode.NotFound, "No Record");
                    return result;
                }
                result.Payload = actionLogs;
            }
            catch (Exception ex) 
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
    }
}
