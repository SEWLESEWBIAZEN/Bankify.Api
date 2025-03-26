using Bankify.Application.Common.DTOs.Auth.Request;
using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Bankify.Application.Features.Commands.Auth{
    public class ChangePassword:IRequest<OperationalResult<string>>{
        public ChangePasswordRequest ChangePasswordRequest{ get; set; }
    }

    internal class ChangePasswordRequestCommandHandler:IRequestHandler<ChangePassword, OperationalResult<string>>
    {
        private readonly IRepositoryBase<BUser> _users;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChangePasswordRequestCommandHandler(INetworkService networkService, IRepositoryBase<BUser> users, IHttpContextAccessor httpContextAccessor)
        {
            _networkService = networkService;
            _users = users;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OperationalResult<string>> Handle(ChangePassword command, CancellationToken cancellationToken)
        {
            var result= new OperationalResult<string>();
            var request= command.ChangePasswordRequest;
            var sessionEmail=_httpContextAccessor.HttpContext.Items["Email"].ToString();

            try
            {
                var dbReachable= await _networkService.IsConnected();
                if(!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error.");
                    return result;
                }

                if(sessionEmail==null){
                    result.AddError(ErrorCode.UnknownError, "Unable to get session user");
                    return result;
                }

                var user= await _users.FirstOrDefaultAsync(us=>us.Email==sessionEmail && us.RecordStatus!=RecordStatus.Deleted);

                if(!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.Password)){
                    result.AddError(ErrorCode.ValidationError, "Current password is incorrect.");
                    return result;
                }

                user.Password=BCrypt.Net.BCrypt.HashPassword(request.NewPassword);                
                user.RegisteredBy=sessionEmail;
                await _users.UpdateAsync(user);
                result.Message="Password Changed Successfully!";

                return result;
            }
            catch(Exception ex){
                result.AddError(ErrorCode.ServerError, ex.Message);
                return result;
            }          

        }
    }
}