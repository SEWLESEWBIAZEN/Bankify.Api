using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
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
        public GetUserByIdHandler(IRepositoryBase<BUser> users)
        {
            _users = users;
        }

        public async Task<OperationalResult<BUser>> Handle(GetUserById request, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<BUser>();
            try
            {
                var user = await _users.FirstOrDefaultAsync(u => u.RecordStatus == RecordStatus.Active && u.Id == request.Id);
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
