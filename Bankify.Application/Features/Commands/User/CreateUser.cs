using Bankify.Application.Common.DTOs.Users.Request;
using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Bankify.Application.Features.Commands.User{
    public class CreateUser:IRequest<OperationalResult<BUser>>
    {
        public CreateUserRequest CreateUserRequest { get; set; }
    }
    internal class  CreateUserCommandHandler:IRequestHandler<CreateUser, OperationalResult<BUser>> 
    {
        private readonly IRepositoryBase<BUser> _users;
        private readonly IFileStorageService _fileStorageService;
        public CreateUserCommandHandler(IFileStorageService fileStorageService, IRepositoryBase<BUser> users)
        {
            _fileStorageService = fileStorageService;
            _users = users;
        }
        public async Task<OperationalResult<BUser>> Handle(CreateUser createUserRequest, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<BUser>();
            var request = createUserRequest.CreateUserRequest;
            var user = new BUser();
            try
            {
                //if the user exist
                var userExist= await _users.ExistWhereAsync(u => u.Email == request.Email ||
                (u.FirstName==request.FirstName &&  u.LastName == request.LastName));
                if(userExist)
                {
                    result.AddError(ErrorCode.RecordFound, "User already exist");
                    return result;
                }
                //saving profile picture to the local folder
                if(request.ProfilePicture != null)
                {
                    var url = await SaveProfilePicture(request.ProfilePicture);
                    user.ProfilePicture= url;                    
                }                
                    user.FirstName = request.FirstName;
                    user.LastName = request.LastName;
                    user.Email = request.Email;
                    user.Password = request.Password;
                    user.PhoneNumber = request.PhoneNumber;
                    user.Address = request.Address; 
                
                //saving user to db
                await _users.AddAsync(user);
                result.Payload = user;
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

