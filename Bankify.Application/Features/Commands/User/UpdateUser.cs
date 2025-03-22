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
    public class UpdateUser:IRequest<OperationalResult<BUser>>
    {     
        public UpdateUserRequest UpdateUserRequest { get; set; }=new UpdateUserRequest();
    }
    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUser, OperationalResult<BUser>>
    {
        private readonly IRepositoryBase<BUser> _users;
        private readonly INetworkService _networkService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IActionLoggerService _actionLoggerService;

        private ISession session;
        public UpdateUserCommandHandler(IRepositoryBase<BUser> users, INetworkService networkService, IFileStorageService fileStorageService, IHttpContextAccessor contextAccessor, IActionLoggerService actionLoggerService)
        {
            _users = users;          
            _networkService = networkService;
            _fileStorageService = fileStorageService;
            _contextAccessor = contextAccessor;
            _actionLoggerService = actionLoggerService;

            session = _contextAccessor.HttpContext.Session;
        }

        public async Task<OperationalResult<BUser>> Handle(UpdateUser updateUserRequest, CancellationToken cancellationToken)
        {
            var sessionUser = session.GetString("user");
            var result = new OperationalResult<BUser>();
            var request = updateUserRequest.UpdateUserRequest;
            try
            {
                var dbAvailable = await _networkService.IsConnected();
                if (!dbAvailable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to Access DB)");
                    return result;
                }
                //if no user Id is provided
                if (request?.Id ==null || request.Id == 0)
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

                //saving profile picture to the local folder
                //removing the previous file
                if (user.ProfilePicture != null) 
                {
                    await _fileStorageService.RemoveFileAsync(user.ProfilePicture);
                }

                if (request.ProfilePicture != null)
                {
                    var url = await SaveProfilePicture(request.ProfilePicture);
                    user.ProfilePicture = url;
                }

                if (request.Password != null) 
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password) ?? BCrypt.Net.BCrypt.HashPassword(user.Password);
                }
              
                user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
                user.Address = request.Address ?? user.Address;
                //update audit
                user.UpdateAudit(sessionUser);

                //saving changes to db
                await _users.UpdateAsync(user);

                //register action log
                await _actionLoggerService.TakeActionLog(ActionType.Update, "User", user.Id, sessionUser, $"User namely: %{user.FirstName} {user.LastName}% was updated on {DateTime.Now} by {sessionUser}");
                 user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password) ?? BCrypt.Net.BCrypt.HashPassword(user.Password);

                result.Payload = user;
                result.Message = "User updated successfully";
            }
            catch (Exception e)
            {
                result.AddError(ErrorCode.ServerError, e.Message);
            }
            return result;
        }
        public async Task<string> SaveProfilePicture(IFormFile profilePicture)
        {
            return await _fileStorageService.UploadFileAsync(profilePicture);
        }
    }

}
