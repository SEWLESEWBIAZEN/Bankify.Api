using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Users;
using MediatR;

namespace Bankify.Application.Features.Commands.User
{
    public class DeleteUser:IRequest<OperationalResult<BUser>>
    {
        public int Id { get; set; }
    }
    internal class DeleteUserCommandHandler : IRequestHandler<DeleteUser, OperationalResult<BUser>>
    {
        private readonly IRepositoryBase<BUser> _users;
        public DeleteUserCommandHandler(IRepositoryBase<BUser> users)
        {
            _users = users;
        }

        public async Task<OperationalResult<BUser>> Handle(DeleteUser request, CancellationToken cancellationToken)
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
                user.RecordStatus = RecordStatus.Deleted;
                await _users.UpdateAsync(user);
                result.Message = "User deleted successfully";
            }
            catch (Exception e)
            {
                result.AddError(ErrorCode.ServerError, e.Message);
            }
            return result;
        }
    }
}
