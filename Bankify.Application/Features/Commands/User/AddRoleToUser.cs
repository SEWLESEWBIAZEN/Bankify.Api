using Bankify.Application.Common.DTOs.Users.Request;
using Bankify.Application.Common.DTOs.Users.Response;
using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Bankify.Application.Features.Commands.User
{
    public class AddRoleToUser:IRequest<OperationalResult<UserRolesDetails>>
    {
      public AddRolesToUserRequest AddRolesToUserRequest { get; set; }
    }

    internal class AddRoleToUserHandler : IRequestHandler<AddRoleToUser, OperationalResult<UserRolesDetails>>
    {
        private readonly IRepositoryBase<UserRole> _userRoles;
        private readonly IRepositoryBase<AppRole> _appRoles;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession session;

        public AddRoleToUserHandler(IRepositoryBase<UserRole> userRoles, INetworkService networkService, IHttpContextAccessor httpContextAccessor, IRepositoryBase<AppRole> appRoles)
        {
            _userRoles = userRoles;
            _networkService = networkService;
            _httpContextAccessor = httpContextAccessor;

            session = _httpContextAccessor.HttpContext.Session;
            _appRoles = appRoles;
        }

        public async Task<OperationalResult<UserRolesDetails>> Handle(AddRoleToUser addRolesToUserRequest, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<UserRolesDetails>();
            var request= addRolesToUserRequest.AddRolesToUserRequest;
            var sessionUser = session.GetString("user");
            try
            {
                var dbAvailable = await _networkService.IsConnected();
                if (!dbAvailable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach Database)");
                    return result;
                }
                var appRoleList=new List<AppRoleDetail>();
                var userRoleList = new List<UserRole>();
                foreach (var roleId in request.RoleIds)
                {
                    if(await _userRoles.ExistWhereAsync(ur=>ur.AppRoleId==roleId && ur.AppUserId == request.UserId))
                    {
                    continue; 
                    }
                    var userRole = new UserRole
                    {
                        AppUserId = request.UserId,
                        AppRoleId = roleId
                    };
                    userRoleList.Add(userRole);                    
                    userRole.Register(sessionUser);
                    var appRole = await _appRoles.FirstOrDefaultAsync(ar => ar.Id == roleId);
                    appRoleList.Add(new AppRoleDetail{ Id=appRole.Id, RoleName=appRole.RoleName, RoleClaims=null});

                }               
                await _userRoles.AddRangeAsync(userRoleList);
                var userRolesDetailsResponse = new UserRolesDetails
                {
                    AppUserId = request.UserId,
                    AppRoles = appRoleList
                };
                result.Payload = userRolesDetailsResponse;
            }
            catch (Exception e)
            {
                result.AddError(ErrorCode.ServerError, e.Message);
            }
            return result;           
        }
    }
}
