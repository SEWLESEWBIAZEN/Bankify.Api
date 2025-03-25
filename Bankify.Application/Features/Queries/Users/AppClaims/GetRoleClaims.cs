using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bankify.Application.Features.Queries.Users.AppClaims
{
    public class GetRoleClaims : IRequest<OperationalResult<List<AppClaim>>>
    {
        public int RoleId { get; set; }

    }
    internal class GetRoleClaimsQueryHandler : IRequestHandler<GetRoleClaims, OperationalResult<List<AppClaim>>>
    {
        private readonly INetworkService _networkService;
        private readonly IRepositoryBase<AppClaim> _appClaims;

        public GetRoleClaimsQueryHandler(INetworkService networkService, IRepositoryBase<AppClaim> appClaims)
        {
            _networkService = networkService;
            _appClaims = appClaims;
        }

        public async Task<OperationalResult<List<AppClaim>>> Handle(GetRoleClaims request, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<List<AppClaim>>();
            try
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Not Connected!");
                    return result;
                }

                var roleClaims = await _appClaims
                .Where(rc => rc.Id != 0)
                .Select(rc => new AppClaim
                {
                    Id = rc.Id,
                    ClaimString = rc.ClaimString,
                    RoleClaims = rc.RoleClaims
                            .Select(ur => new RoleClaim { AppRoleId = ur.AppRoleId, AppClaimId = ur.AppClaimId })
                            .ToList()
                })
                .ToListAsync();

                var RoleClaims = roleClaims.Where(ap => ap.RoleClaims.Any(ur => ur.AppRoleId == request.RoleId)).ToList();
                if(RoleClaims.Count == 0){
                     result.Payload=new List<AppClaim>();
                     return result;
                }
                result.Payload=RoleClaims;
                return result;
            }
            catch (Exception e)
            {
                result.AddError(ErrorCode.ServerError, e.Message);
                return result;
            }

        }
    }
}