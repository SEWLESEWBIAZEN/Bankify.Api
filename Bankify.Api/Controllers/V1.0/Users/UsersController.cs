using Bankify.Application.Common.DTOs.Users.Request;
using Bankify.Application.Features.Commands.User;
using Bankify.Application.Features.Queries.Users;
using Bankify.Domain.Models.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Bankify.Api.Controllers.V1._0.Users
{
    public class UsersController : BaseController
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(RecordStatus? recordStatus)
        {
            var query = new GetAllUsers { RecordStatus = recordStatus };
            var result = await _mediator.Send(query);
            return result.IsError? HandleErrorResponse(result.Errors) : Ok(result.Payload);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] CreateUserRequest createUserRequest)
        {
            var command = new CreateUser { CreateUserRequest = createUserRequest };
            var result = await _mediator.Send(command);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result.Payload);
        }
    }
}
