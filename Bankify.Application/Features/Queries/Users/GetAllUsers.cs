using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        public GetAllUsersQueryHandler(IRepositoryBase<BUser> users, INetworkService networkService)
        {
            _users = users;
            _networkService = networkService;
        }
        public async Task<OperationalResult<List<BUser>>> Handle(GetAllUsers request, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<List<BUser>>();
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
                    RecordStatus.Active=> await _users.Where(u => u.RecordStatus==RecordStatus.Active).ToListAsync(),
                    RecordStatus.InActive=>await _users.Where(u=>u.RecordStatus==RecordStatus.InActive).ToListAsync(),
                    _=>await _users.Where(u => u.RecordStatus == RecordStatus.Active).ToListAsync()

                };
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
