using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Bankify.Application.Features.Commands.User
{
    public class AddRoleToUser:IRequest<OperationalResult<UserRole>>
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }

    internal class AddRoleToUserHandler : IRequestHandler<AddRoleToUser, OperationalResult<UserRole>>
    {
        private readonly IRepositoryBase<UserRole> _userRoles;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession session; 

        public AddRoleToUserHandler(IRepositoryBase<UserRole> userRoles, INetworkService networkService, IHttpContextAccessor httpContextAccessor)
        {
            _userRoles = userRoles;
            _networkService = networkService;
            _httpContextAccessor = httpContextAccessor;

            session= _httpContextAccessor.HttpContext.Session;
        }

        public async Task<OperationalResult<UserRole>> Handle(AddRoleToUser request, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<UserRole>();
            var sessionUser = session.GetString("user");
            try
            {
                var dbAvailable = await _networkService.IsConnected();
                if (!dbAvailable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach Database)");
                    return result;
                }
                var userRole = new UserRole
                {
                    AppUserId = request.UserId,
                    AppRoleId = request.RoleId
                };
                userRole.Register(sessionUser);
                await _userRoles.AddAsync(userRole);
                result.Payload = userRole;
            }
            catch (Exception e)
            {
                result.AddError(ErrorCode.ServerError, e.Message);
            }
            return result;           
        }
    }
}
