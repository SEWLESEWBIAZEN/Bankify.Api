using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Bankify.Application.Features.Queries.Users
{
    public class GetAllUsers:IRequest<OperationalResult<List<BUser>>>
    {
        public RecordStatus? RecordStatus;
    }
    internal class GetAllUsersQueryHandler : IRequestHandler<GetAllUsers, OperationalResult<List<BUser>>>
    {
        private readonly IRepositoryBase<BUser> _users;
        private readonly INetworkService _networkService;
        private readonly IConfiguration _configuration;
        public GetAllUsersQueryHandler(IRepositoryBase<BUser> users, INetworkService networkService, IConfiguration configuration)
        {
            _users = users;
            _networkService = networkService;
            _configuration = configuration;
        }
        public async Task<OperationalResult<List<BUser>>> Handle(GetAllUsers request, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<List<BUser>>();
            var serverUrl = _configuration["SftpSettings:BaseUrl"];
            
            try 
            {
                var dbAvailable = await _networkService.IsConnected();
                if (!dbAvailable) 
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to Access DB)");
                    return result;
                }
                var registeredUsers = request.RecordStatus switch
                {
                    RecordStatus.Active=> await _users.Where(u => u.RecordStatus==RecordStatus.Active, "UserRoles.AppRole.RoleClaims.AppClaim", "Accounts.AccountType").ToListAsync(),
                    RecordStatus.InActive=>await _users.Where(u=>u.RecordStatus==RecordStatus.InActive,"UserRoles.AppRole.RoleClaims.AppClaim","Accounts.AccountType").ToListAsync(),
                    _=>await _users.Where(u => u.RecordStatus == RecordStatus.Active,"UserRoles.AppRole.RoleClaims.AppClaim","Accounts.AccountType").ToListAsync()

                };

                foreach( var user in registeredUsers)
                {
                    if (user.ProfilePicture != null)
                    {
                        user.ProfilePicture = (serverUrl + "/" + user.ProfilePicture);
                    }                  
                }

                if(registeredUsers.Count==0)
                {
                    result.AddError(ErrorCode.NotFound, "No registered users found");                    
                }
                else
                {
                   result.Payload = registeredUsers;
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
