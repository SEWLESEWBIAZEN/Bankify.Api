using Bankify.Application.Common.DTOs.Users.Request;
using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Users;
using MediatR;

namespace Bankify.Application.Features.Commands.User
{
    public class UpdateUser:IRequest<OperationalResult<BUser>>
    {     
        public UpdateUserRequest UpdateUserRequest { get; set; }
    }
    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUser, OperationalResult<BUser>>
    {
        private readonly IRepositoryBase<BUser> _users;
        public UpdateUserCommandHandler(IRepositoryBase<BUser> users)
        {
            _users = users;
        }

        public async Task<OperationalResult<BUser>> Handle(UpdateUser updateUserRequest, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<BUser>();
            var request = updateUserRequest.UpdateUserRequest;
            try
            {
                //if no user Id is provided
                if (request.Id ==null || request.Id == 0)
                {
                    result.AddError(ErrorCode.EmptyRquest, "User Id is required");
                    return result;
                }
                
                //fetching user
                var user = await _users.FirstOrDefaultAsync(u => u.RecordStatus == RecordStatus.Active && u.Id == updateUserRequest.UpdateUserRequest.Id);
                
                //if user not found
                if (user == null)
                {
                    result.AddError(ErrorCode.NotFound, "User not found");
                    return result;
                }
                //updating user
                user.FirstName = request.FirstName ?? user.FirstName;
                user.LastName = request.LastName ?? user.LastName;
                user.Email = request.Email ?? user.Email;
                user.Password = request.Password ?? user.Password;
                user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
                user.Address = request.Address ?? user.Address;
                
                //saving changes to db
                await _users.UpdateAsync(user);
                result.Payload = user;
                result.Message = "User updated successfully";
            }
            catch (Exception e)
            {
                result.AddError(ErrorCode.ServerError, e.Message);
            }
            return result;
        }
    }

}
