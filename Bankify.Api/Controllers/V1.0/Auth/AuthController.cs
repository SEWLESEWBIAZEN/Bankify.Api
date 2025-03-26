using Bankify.Application.Common.DTOs.Auth.Request;
using Bankify.Application.Common.DTOs.Auth.Response;
using Bankify.Application.Features.Commands.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Bankify.Api.Controllers.V1._0.Auth
{
    public class AuthController : BaseController
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var command = new UserLogin { LoginRequest = loginRequest };
            var result = await _mediator.Send(command);
            var loginResponse = _mapper.Map<LoginResponseDetail>(result.Payload);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(loginResponse);

        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest ){
            var command = new ChangePassword {ChangePasswordRequest=changePasswordRequest };
            var result = await _mediator.Send(command);
            return result.IsError? HandleErrorResponse(result.Errors) : Ok(result);
        }

    }
    
}
