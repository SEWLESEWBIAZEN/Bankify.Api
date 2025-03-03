using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Bankify.Application.Features.Commands.User
{
    public class DeleteUser:IRequest<OperationalResult<BUser>>
    {
        public int Id { get; set; }
    }
    internal class DeleteUserCommandHandler : IRequestHandler<DeleteUser, OperationalResult<BUser>>
    {
        private readonly IRepositoryBase<BUser> _users;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActionLoggerService _actionLoggerService;
        private ISession session;
        public DeleteUserCommandHandler(IRepositoryBase<BUser> users, INetworkService networkService, IHttpContextAccessor httpContextAccessor, IActionLoggerService actionLoggerService)
        {
            _users = users;
            _networkService = networkService;
            _httpContextAccessor = httpContextAccessor;
            session = _httpContextAccessor.HttpContext.Session;
            _actionLoggerService = actionLoggerService;
        }

        public async Task<OperationalResult<BUser>> Handle(DeleteUser request, CancellationToken cancellationToken)
        {
            var sessionUser = session.GetString("user");
            var result = new OperationalResult<BUser>();
            try
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach Database");
                    return result;                    
                }
                var user = await _users.FirstOrDefaultAsync(u => u.RecordStatus == RecordStatus.Active && u.Id == request.Id);
                if (user == null)
                {
                    result.AddError(ErrorCode.NotFound, "User not found");
                    return result;
                }
                user.RecordStatus = RecordStatus.Deleted;
                //update audit
                user.UpdateAudit(sessionUser);
                await _users.UpdateAsync(user);
                await _actionLoggerService.TakeActionLog(ActionType.Delete, "User", user.Id, sessionUser, $"User namely: %{user.FirstName} {user.LastName}% was deleted on {DateTime.Now} by {sessionUser}");
                result.Message = "User deleted successfully";
            }
            catch (Exception e)
            {
                result.AddError(ErrorCode.ServerError, e.Message);
            }
            return result;
        }
    }
}
