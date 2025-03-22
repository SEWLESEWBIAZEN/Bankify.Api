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
      public AddRolesToUserRequest AddRolesToUserRequest { get; set; }=new AddRolesToUserRequest();
    }

    internal class AddRoleToUserHandler : IRequestHandler<AddRoleToUser, OperationalResult<UserRolesDetails>>
    {
        private readonly IRepositoryBase<UserRole> _userRoles;
        private readonly IRepositoryBase<AppRole> _appRoles;
        private readonly INetworkService _networkService;
        private readonly IActionLoggerService _actionLoggerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession session;

        public AddRoleToUserHandler(IRepositoryBase<UserRole> userRoles, INetworkService networkService, IHttpContextAccessor httpContextAccessor, IRepositoryBase<AppRole> appRoles, IActionLoggerService actionLoggerService)
        {
            _userRoles = userRoles;
            _networkService = networkService;
            _httpContextAccessor = httpContextAccessor;

            session = _httpContextAccessor.HttpContext.Session;
            _appRoles = appRoles;
            _actionLoggerService = actionLoggerService;
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
                var roleNames=new List<string>();
                var appRoleList=new List<AppRoleDetail>();
                var userRoleList = new List<UserRole>();

                //romoving an existing roles
                var userRoles = await _userRoles.WhereAsync(ur => ur.AppUserId == request.UserId);
                _userRoles.RemoveRange(userRoles);
               

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
                    roleNames.Add(appRole.RoleName);
                   
                    appRoleList.Add(new AppRoleDetail{ Id=appRole.Id, RoleName=appRole.RoleName, RoleClaims=null});

                }               
                await _userRoles.AddRangeAsync(userRoleList);
                await _actionLoggerService.TakeActionLog(ActionType.Grant, "User",
                       request.UserId, sessionUser, $"{String.Join(",", roleNames)} has been added to user with Id {request.UserId}");
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
