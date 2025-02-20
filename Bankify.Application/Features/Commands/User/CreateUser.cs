using Bankify.Application.Common.DTOs.Users.Request;
using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Bankify.Application.Features.Commands.User
{
    public class CreateUser : IRequest<OperationalResult<BUser>>
    {
        public CreateUserRequest CreateUserRequest { get; set; }
    }
    internal class CreateUserCommandHandler : IRequestHandler<CreateUser, OperationalResult<BUser>>
    {
        private readonly IRepositoryBase<BUser> _users;
        private readonly IFileStorageService _fileStorageService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INetworkService _networkService;
        private readonly IActionLoggerService _actionLoggerService;

        private ISession session=>_contextAccessor.HttpContext.Session;
        public CreateUserCommandHandler(IFileStorageService fileStorageService, IRepositoryBase<BUser> users, IHttpContextAccessor contextAccessor, INetworkService networkService, IActionLoggerService actionLoggerService)
        {
            _fileStorageService = fileStorageService;
            _users = users;
            _contextAccessor = contextAccessor;
            _networkService = networkService;
            _actionLoggerService = actionLoggerService;
        }
        public async Task<OperationalResult<BUser>> Handle(CreateUser createUserRequest, CancellationToken cancellationToken)
        {
            var sessionUser=session.GetString("user");
            var result = new OperationalResult<BUser>();
            var request = createUserRequest.CreateUserRequest;
            var user = new BUser();
            try
            {
                var dbAvailable = await _networkService.IsConnected();
                if (!dbAvailable) 
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable reach Database)");
                    return result;
                }
                //if the user exist
                var userExist = await _users.ExistWhereAsync(u => u.Email == request.Email ||
                (u.FirstName == request.FirstName && u.LastName == request.LastName));
                if (userExist)
                {
                    result.AddError(ErrorCode.RecordFound, "User already exist");
                    return result;
                }
                //saving profile picture to the local folder
                if (request.ProfilePicture != null)
                {
                    var url = await SaveProfilePicture(request.ProfilePicture);
                    user.ProfilePicture = url;
                }
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Email = request.Email;
                user.Password = request.Password;
                user.PhoneNumber = request.PhoneNumber;
                user.Address = request.Address;

                //update registered by
                user.Register(sessionUser);

                //saving user to db
                await _users.AddAsync(user);               
                result.Payload = user; 
                await _actionLoggerService.TakeActionLog(ActionType.Create, "User", user.Id, sessionUser, $"New User namely: %{user.FirstName} {user.LastName}% was created on {DateTime.Now} by {sessionUser}");
                result.Message = "User created successfully";
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
        public async Task<string> SaveProfilePicture(IFormFile profilePicture)
        {
            return await _fileStorageService.UploadFileAsync(profilePicture);
        }
    }


}

