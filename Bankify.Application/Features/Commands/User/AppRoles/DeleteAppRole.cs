using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Bankify.Application.Features.Commands.User.AppRoles
{
    public class DeleteAppRole : IRequest<OperationalResult<AppRole>>
    {
        public int Id { get; set; }
    }

    internal class DeleteAppRoleCommandHandler : IRequestHandler<DeleteAppRole, OperationalResult<AppRole>>
    {
        private readonly IRepositoryBase<AppRole> _roles;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession session;

        public DeleteAppRoleCommandHandler(IRepositoryBase<AppRole> roles, INetworkService networkService, IHttpContextAccessor httpContextAccessor)
        {
            _roles = roles;
            _networkService = networkService;
            _httpContextAccessor = httpContextAccessor;
            session=_httpContextAccessor.HttpContext.Session;
        }

        public async Task<OperationalResult<AppRole>> Handle(DeleteAppRole command, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<AppRole>();
            var sessionUser=session.GetString("user");
            try
            {
                var dbReachable=await _networkService.IsConnected();
                if(!dbReachable){
                    result.AddError(ErrorCode.NetworkError, "Network error.");
                    return result;
                }

                if(command.Id==0){
                    result.AddError(ErrorCode.NotFound, "Empty Id Sent.");
                    return result;
                }

                var role=await _roles.FirstOrDefaultAsync(r=>r.Id==command.Id && r.RecordStatus!=RecordStatus.Deleted);
                if(role==null){
                    result.AddError(ErrorCode.NotFound,"role doesn't exist");
                    return result;
                }

                role.RecordStatus=RecordStatus.Deleted;
                role.UpdateAudit(sessionUser);
                await _roles.UpdateAsync(role);
                result.Message="Role deleted successfully";
                return result;

            }
            catch(Exception ex){
                result.AddError(ErrorCode.ServerError, ex.Message);
                return result;
            }
            
        }
    }
}