using Bankify.Application.Common.DTOs.Users.Request;
using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Domain.Models.Users;
using MediatR;

namespace Bankify.Application.Features.Commands.User
{
    public class CreateUser:IRequest<OperationalResult<BUser>>
    {
        public CreateUserRequest CreateUserRequest { get; set; }
    }

    internal class  CreateUserCommandHandler:IRequestHandler<CreateUser, OperationalResult<BUser>> 
    {
        private readonly IRepositoryBase<BUser> _users;
        public CreateUserCommandHandler(IRepositoryBase<BUser> users)
        {
            _users = users;
        }

        public async Task<OperationalResult<BUser>> Handle(CreateUser request, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<BUser>();
            try
            {
                var user = new BUser
                {
                    FirstName = request.CreateUserRequest.FirstName,
                    LastName = request.CreateUserRequest.LastName,
                    Email = request.CreateUserRequest.Email,
                    Password = request.CreateUserRequest.Password,
                    PhoneNumber = request.CreateUserRequest.PhoneNumber,
                    Address = request.CreateUserRequest.Address                    
                };
                await _users.AddAsync(user);
                result.Payload = user;
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
    }
}
