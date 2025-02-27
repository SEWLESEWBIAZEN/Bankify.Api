using Bankify.Application.Common.DTOs.Users.Request.AppRoles;
using Bankify.Application.Features.Commands.User.AppRoles;
using Bankify.Application.Features.Queries.Users.AppRoles;
using Microsoft.AspNetCore.Mvc;

namespace Bankify.Api.Controllers.V1._0.Users
{
    public class AppRoleController : BaseController
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllAppRoles();
            var result = await _mediator.Send(query);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result.Payload);
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateAppRoleRequest createAppRoleRequest)
        {
            var command = new CreateAppRole { CreateAppRoleRequest = createAppRoleRequest };
            var result = await _mediator.Send(command);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result);

        }

        [HttpPut("GrantPrivilege")]
        public async Task<IActionResult> GrantPrivilege([FromBody] GrantPrivilegeRequest grantPrivilegeRequest)
        {
            var command = new GrantPrivilege { GrantPrivilegeRequest = grantPrivilegeRequest };
            var result = await _mediator.Send(command);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result.Payload);

        }
    }
}
