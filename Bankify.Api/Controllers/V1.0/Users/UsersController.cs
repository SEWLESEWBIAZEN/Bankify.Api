using Bankify.Application.Common.DTOs.Users.Request;
using Bankify.Application.Common.DTOs.Users.Response;
using Bankify.Application.Features.Commands.User;
using Bankify.Application.Features.Queries.Users;
using Bankify.Domain.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bankify.Api.Controllers.V1._0.Users
{
    public class UsersController : BaseController
    {
        [Authorize(Roles = "Super Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(RecordStatus? recordStatus)
        {
            var query = new GetAllUsers { RecordStatus = recordStatus };
            var result = await _mediator.Send(query);
            var usersList=_mapper.Map<List<UserDetail>>(result.Payload);
            return result.IsError? HandleErrorResponse(result.Errors) : Ok(usersList);
        }

       
        [Authorize(Roles = "Admin")]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetUserById { Id = id };
            var result = await _mediator.Send(query);
            var userDetail = _mapper.Map<UserDetail>(result.Payload);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(userDetail);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddRoleToUser")]
        public async Task<IActionResult> AddRoleToUser([FromBody] AddRolesToUserRequest addRolesToUserRequest)
        {
            var command = new AddRoleToUser { AddRolesToUserRequest=addRolesToUserRequest };
            var result = await _mediator.Send(command);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] CreateUserRequest createUserRequest)
        {
            var command = new CreateUser { CreateUserRequest = createUserRequest };
            var result = await _mediator.Send(command);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromForm] UpdateUserRequest updateUserRequest)
        {
            var command = new UpdateUser { UpdateUserRequest = updateUserRequest };
            var result = await _mediator.Send(command);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result.Payload);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteUser { Id = id };
            var result = await _mediator.Send(command);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result);
        }
    }
}
