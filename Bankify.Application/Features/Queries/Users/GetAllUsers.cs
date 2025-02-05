using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
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
        public GetAllUsersQueryHandler(IRepositoryBase<BUser> users)
        {
            _users = users;
        }
        public async Task<OperationalResult<List<BUser>>> Handle(GetAllUsers request, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<List<BUser>>();
            try 
            {
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
