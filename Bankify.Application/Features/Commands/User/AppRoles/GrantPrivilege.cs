using Bankify.Application.Common.DTOs.Users.Response;
using Bankify.Application.Common.Helpers;
using Bankify.Application.Features.Queries.Users.AppRoles;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Bankify.Application.Features.Commands.User.AppRoles
{
    public class GrantPrivilege:IRequest<OperationalResult<RoleClaimDetails>>
    {
        public GrantPrivilegeRequest GrantPrivilegeRequest { get; set; }
    }

   internal class GrantPrivilegeCommandHandler:IRequestHandler<GrantPrivilege, OperationalResult<RoleClaimDetails>>
    {
        private readonly IRepositoryBase<RoleClaim> _roleClaims;
        private readonly IRepositoryBase<AppRole> _appRoles;
        private readonly IRepositoryBase<AppClaim> _appClaims;
        private readonly IActionLoggerService _actionLoggerService;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private ISession session;

        public GrantPrivilegeCommandHandler(IRepositoryBase<RoleClaim> roleClaims, INetworkService networkService, IHttpContextAccessor httpContextAccessor, IRepositoryBase<AppRole> appRoles, IRepositoryBase<AppClaim> appClaims, IActionLoggerService actionLoggerService)
        {
            _roleClaims = roleClaims;
            _networkService = networkService;
            _httpContextAccessor = httpContextAccessor;

            session = _httpContextAccessor.HttpContext.Session;
            _appRoles = appRoles;
            _appClaims = appClaims;
            _actionLoggerService = actionLoggerService;
        }

        public async Task<OperationalResult<RoleClaimDetails>>  Handle(GrantPrivilege command, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<RoleClaimDetails>();
            var request = command.GrantPrivilegeRequest;
            var sessionUser = session.GetString("user");

            try 
            {
                if(request.AppClaimsId is null || request.AppClaimsId.Count == 0 || request.AppRoleId==0)
                {
                    result.AddError(ErrorCode.EmptyRquest, "Empty/InComplete Request Sent.");
                    return result;
                }
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach database.)");
                    return result;
                }
                var role=await _appRoles.FirstOrDefaultAsync(ar=>ar.RecordStatus!=RecordStatus.Deleted && ar.Id==request.AppRoleId);
                if(role == null)
                {
                    result.AddError(ErrorCode.NotFound, "Role is Not Found!");
                    return result;
                }
                var newRoleClaims=new List<RoleClaim>();
                var newAppClaimsDetail=new List<AppClaimDetail>();
                var claimNames = new List<string>();

                foreach(var claimId in request.AppClaimsId)
                {
                    //check if the claim is either already added to a role or does not exist in the db.
                    var claim = await _appClaims.FirstOrDefaultAsync(ac => ac.Id == claimId);
                    var roleClaimAlreadyExists=await _roleClaims.ExistWhereAsync(rc=>rc.AppClaimId==claimId && rc.AppRoleId==request.AppRoleId);
                    if (claim != null &&!roleClaimAlreadyExists)
                    {
                        //create new role claim instance to be added 
                        var newRoleClaim = new RoleClaim { AppRoleId = request.AppRoleId, AppClaimId = claimId, RegisteredBy=sessionUser };
                        //retrieve each cla
                       
                        newRoleClaims.Add(newRoleClaim);

                        //add claim to claim names list
                        claimNames.Add(claim.ClaimString);                       
                        var newAppClaimDetail = new AppClaimDetail {Id = claim.Id, ClaimString = claim.ClaimString };
                        newAppClaimsDetail.Add(newAppClaimDetail);                      

                    }
                    else { continue; }                  
                }
               var grantSuccess= await _roleClaims.AddRangeAsync(newRoleClaims);
                if (grantSuccess)
                {
                    //take an action log for audit
                    await _actionLoggerService.TakeActionLog(ActionType.Grant,
                        "Role", role.Id, sessionUser, $"{claimNames.ToString()} has been added to role {role.RoleName}");

                    result.Message = "Claims Are added to a Role";
                }
                else
                {
                    result.Message = "Adding Claims to a Role Failed";
                }
                var newRoleClaimsDetail = new RoleClaimDetails { AppRoleId = request.AppRoleId, AppClaims = newAppClaimsDetail };
                result.Payload= newRoleClaimsDetail;

            }
            catch (Exception ex) 
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
       
            return result;
        }



    }
}
