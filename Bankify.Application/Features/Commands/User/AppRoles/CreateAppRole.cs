using Bankify.Application.Common.DTOs.Users.Request.AppRoles;
using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Users;
using MediatR;

namespace Bankify.Application.Features.Commands.User.AppRoles
{
    public class CreateAppRole : IRequest<OperationalResult<AppRole>>
    {
        public CreateAppRoleRequest CreateAppRoleRequest { get; set; }=new CreateAppRoleRequest();
    }
    internal class CreateAppRoleCommandHandler : IRequestHandler<CreateAppRole, OperationalResult<AppRole>>
    {
        private readonly IRepositoryBase<AppRole> _appRoles;
        private readonly INetworkService _networkService;

        public CreateAppRoleCommandHandler(IRepositoryBase<AppRole> appRoles, INetworkService networkService)
        {
            _appRoles = appRoles;
            _networkService = networkService;
        }

        public async Task<OperationalResult<AppRole>> Handle(CreateAppRole command, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<AppRole>();
            var request = command.CreateAppRoleRequest;
            try
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach database)");
                    return result;
                }

                if (request.AppRoleName is null)
                {
                    result.AddError(ErrorCode.EmptyRquest, "Empty Role Properties Sent");
                    return result;
                }

                var role = await _appRoles.FirstOrDefaultAsync(rc => rc.RoleName == request.AppRoleName);
                if (role!=null && role.RecordStatus!=RecordStatus.Active)
                {
                    role.RecordStatus = RecordStatus.Active;
                    await _appRoles.UpdateAsync(role);
                    result.Message= "Role Added";
                    return result;
                }

                var newRole = new AppRole
                {
                    RoleName = request.AppRoleName
                };
                var createRoleSuccess = await _appRoles.AddAsync(newRole);
                if (createRoleSuccess)
                {
                    result.Message = "New Role registered Successfully";
                }
                else
                {
                    result.Message = "Create New Role  is Failed";
                    result.Payload = newRole;
                }
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;

        }
    }
}
