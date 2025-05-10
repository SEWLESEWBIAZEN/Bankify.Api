using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Users;
using MediatR;

namespace Bankify.Application.Features.Queries.Users
{
    public class GetUserById:IRequest<OperationalResult<BUser>>
    {
        public int Id { get; set; }
    }

    internal class GetUserByIdHandler : IRequestHandler<GetUserById, OperationalResult<BUser>>
    {
        private readonly IRepositoryBase<BUser> _users;
        private readonly INetworkService _networkService;
        public GetUserByIdHandler(IRepositoryBase<BUser> users, INetworkService networkService)
        {
            _users = users;
            _networkService = networkService;
        }

        public async Task<OperationalResult<BUser>> Handle(GetUserById request, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<BUser>();
            try
            {
                var dbAvailable = await _networkService.IsConnected();
                if (!dbAvailable) 
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach Database)");
                    return result;
                }
                var user = await _users.FirstOrDefaultAsync(u => u.RecordStatus == RecordStatus.Active && u.Id == request.Id, "UserRoles.AppRole.RoleClaims.AppClaim", "Accounts.AccountType");
                if (user == null)
                {
                    result.AddError(ErrorCode.NotFound, "User not found");
                    return result;
                }
                result.Payload = user;
            }
            catch (Exception e)
            {
                result.AddError(ErrorCode.ServerError, e.Message);
            }            
            
            return result;
        }
    }
}
