using Bankify.Application.Common.DTOs.Users.Request;
using Bankify.Application.Common.DTOs.Users.Response;
using Bankify.Application.Features.Commands.User;
using Bankify.Application.Features.Queries.Users;
using Bankify.Application.Features.Queries.Users.AppClaims;
using Microsoft.AspNetCore.Mvc;

namespace Bankify.Api.Controllers.V1._0.Users
{
    public class AppClaimController : BaseController
    {

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllRoleClaims();
            var result=await _mediator.Send(query); 
            return result.IsError?HandleErrorResponse(result.Errors):Ok(result.Payload);
        }

        [HttpGet("GetClaimsByRole")]
        public async Task<IActionResult> GetClaimsByRole(int roleId){
            var query= new GetRoleClaims{RoleId = roleId};
            var result = await _mediator.Send(query);
            var claimList =_mapper.Map<List<AppClaimDetail>>(result.Payload);
            return result.IsError?HandleErrorResponse(result.Errors):Ok(claimList);
        } 

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateRoleClaimRequest createRoleClaimRequest)
        {
            var command = new CreateRoleClaim { CreateRoleClaimRequest = createRoleClaimRequest };
            var result=await _mediator.Send(command);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteRoleClaim { Id = id };
            var result= await _mediator.Send(command);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result);
        }
    }
}
