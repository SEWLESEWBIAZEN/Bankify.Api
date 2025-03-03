using Bankify.Application.Common.DTOs.Accounts.Request;
using Bankify.Application.Common.DTOs.Users.Request;
using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Users;
using MediatR;

namespace Bankify.Application.Features.Commands.User
{
    public class CreateRoleClaim:IRequest<OperationalResult<AppClaim>>
    {
        public CreateRoleClaimRequest CreateRoleClaimRequest { get; set; }
    }
    internal class CreateRoleClaimCommandHandler:IRequestHandler<CreateRoleClaim, OperationalResult<AppClaim>>
    {
        private readonly IRepositoryBase<AppClaim> _roleClaims;
        private readonly INetworkService _networkService;

        public CreateRoleClaimCommandHandler(IRepositoryBase<AppClaim> roleClaims, INetworkService networkService)
        {
            _roleClaims = roleClaims;
            _networkService = networkService;
        }

        public async Task<OperationalResult<AppClaim>> Handle(CreateRoleClaim command, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<AppClaim>();
            var request = command.CreateRoleClaimRequest;
            try 
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach database)");
                    return result;
                }

                if(request.ClaimName is null)
                {
                    result.AddError(ErrorCode.EmptyRquest, "Empty Claim Properties Sent");
                    return result;
                }

                var claimAlreadyExists= await _roleClaims.ExistWhereAsync(rc=>rc.ClaimString==request.ClaimName);
                if (claimAlreadyExists) 
                {
                    result.AddError(ErrorCode.RecordExists, "Claim Already Exists");
                    return result;
                }

                var newClaim = new AppClaim
                {
                    ClaimString=request.ClaimName
                };               
                var createClaimSuccess=await _roleClaims.AddAsync(newClaim);
                if (createClaimSuccess) 
                {
                    result.Message = "New App Claim registered Successfully";
                }
                else
                {
                    result.Message = "Create New App Claim is Failed";
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
