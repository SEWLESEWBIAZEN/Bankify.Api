using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Users;
using MediatR;

namespace Bankify.Application.Features.Queries.Users
{
    public class GetAllRoleClaims:IRequest<OperationalResult<List<AppClaim>>>
    {
    }

    internal class GetAllRoleClaimsQueryHandler:IRequestHandler<GetAllRoleClaims, OperationalResult<List<AppClaim>>>
    {
        private readonly IRepositoryBase<AppClaim> _roleClaims;
        private readonly INetworkService _networkService;

        public GetAllRoleClaimsQueryHandler(IRepositoryBase<AppClaim> roleClaims, INetworkService networkService)
        {
            _roleClaims = roleClaims;
            _networkService = networkService;
        }

        public async Task<OperationalResult<List<AppClaim>>> Handle(GetAllRoleClaims query, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<List<AppClaim>>();
            try 
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable) 
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error (Unable to reach database)");
                    return result;
                }
                var roleClaims=await _roleClaims.WhereAsync(rc=>rc.Id!=0);
                if (roleClaims.Count == 0)
                {
                    result.AddError(ErrorCode.NotFound, "No Claim Found"); 
                    return result;
                }
                result.Payload = roleClaims;

            }
            catch (Exception ex) 
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
    }
}
