using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bankify.Application.Features.Queries.Users.AppRoles
{
    public class GetAllUserRoles : IRequest<OperationalResult<List<AppRole>>>
    {
        public int UserId { get; set; }

    }

    internal class GetAllUserRolesQueryHandler : IRequestHandler<GetAllUserRoles, OperationalResult<List<AppRole>>>
    {
        private readonly IRepositoryBase<AppRole> _AppRoles;
        private readonly INetworkService _networkService;

        public GetAllUserRolesQueryHandler(IRepositoryBase<AppRole> AppRoles, INetworkService networkService)
        {
            _AppRoles = AppRoles;
            _networkService = networkService;
        }

        public async Task<OperationalResult<List<AppRole>>> Handle(GetAllUserRoles query, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<List<AppRole>>();
            try
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error (Unable to reach database)");
                    return result;
                }

                var appRoles = await _AppRoles
                .Where(rc=> rc.RecordStatus != RecordStatus.Deleted)
                .Select(rc => new AppRole
                    {
                        Id = rc.Id,
                        RoleName = rc.RoleName, 
                        UserRoles = rc.UserRoles
                            .Select(ur => new UserRole { AppUserId=ur.AppUserId,AppRoleId= ur.AppRoleId })
                            .ToList()
                    })
                .ToListAsync();

                var UserRoles = appRoles.Where(ap => ap.UserRoles.Any(ur => ur.AppUserId == query.UserId)).ToList();

                if (UserRoles.Count == 0)
                {
                    result.AddError(ErrorCode.NotFound, "No Role Found");
                    return result;
                }
                result.Payload = UserRoles;

            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
    }
}
