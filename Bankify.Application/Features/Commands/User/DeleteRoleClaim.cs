using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Bankify.Application.Features.Commands.User
{
    public class DeleteRoleClaim:IRequest<OperationalResult<AppClaim>>
    {
        public int Id { get; set; }
    }

    internal class DeleteRoleClaimCommandHandler:IRequestHandler<DeleteRoleClaim, OperationalResult<AppClaim>>
    {
        private readonly IRepositoryBase<AppClaim> _roleClaims;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteRoleClaimCommandHandler(IRepositoryBase<AppClaim> roleClaims, INetworkService networkService, IHttpContextAccessor httpContextAccessor)
        {
            _roleClaims = roleClaims;
            _networkService = networkService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OperationalResult<AppClaim>> Handle(DeleteRoleClaim request, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<AppClaim>();
            var sessionUser=_httpContextAccessor.HttpContext.Items["UserName"].ToString();
            try 
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error (Unable to reach database)");
                    return result;
                }

                var roleClaim= await _roleClaims.FirstOrDefaultAsync(rc=>rc.Id==request.Id);
                if (roleClaim is null)
                {
                    result.AddError(ErrorCode.NotFound, "Claim is Not Found");
                    return result;
                }                
                 _roleClaims.Remove(roleClaim);
                result.Message = "Claim Deleted/Removed";
            }
            catch (Exception ex) 
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            
            return result;
        }
    }
}
