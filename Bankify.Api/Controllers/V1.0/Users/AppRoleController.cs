using Bankify.Application.Common.DTOs.Users.Request.AppRoles;
using Bankify.Application.Common.DTOs.Users.Response;
using Bankify.Application.Features.Commands.User.AppRoles;
using Bankify.Application.Features.Queries.Users.AppRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bankify.Api.Controllers.V1._0.Users
{
    public class AppRoleController : BaseController
    {

        [Authorize(Roles = "Super Admin, Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(bool forDropdown=false)
        {
            var query = new GetAllAppRoles { ForDropdown =forDropdown};
            var result = await _mediator.Send(query);
            var rolesList=_mapper.Map<List<AppRoleDetail>>(result.Payload);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(rolesList);
        }

        [Authorize(Roles = "Super Admin, Admin")]
        [HttpGet("GetRolesByUser")]
        public async Task<IActionResult> GetRolesByUser(int userId)
        {
            var query = new GetAllUserRoles { UserId=userId};
            var result = await _mediator.Send(query);
            var rolesList = _mapper.Map<List<AppRoleDetail>>(result.Payload);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(rolesList);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateAppRoleRequest createAppRoleRequest)
        {
            var command = new CreateAppRole { CreateAppRoleRequest = createAppRoleRequest };
            var result = await _mediator.Send(command);
            var appRole=_mapper.Map<AppRoleDetail>(result.Payload);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(appRole);

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
