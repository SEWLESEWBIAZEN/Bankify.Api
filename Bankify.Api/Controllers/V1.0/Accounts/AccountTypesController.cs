using Bankify.Application.Common.DTOs.AccountTypes.Request;
using Bankify.Application.Features.Commands.AccountTypes;
using Bankify.Application.Features.Queries.AccountTypes;
using Bankify.Domain.Models.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Bankify.Api.Controllers.V1._0.Accounts
{
    public class AccountTypesController : BaseController
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(RecordStatus? recordStatus)
        {
            var query = new GetAllAccountTypes { RecordStatus = recordStatus };
            var result = await _mediator.Send(query);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result.Payload);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetAccountTypeById { Id = id };
            var result = await _mediator.Send(query);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result.Payload);

        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateAccountTypeRequest createAccountTypeRequest)
        {
            var command = new CreateAccountType { CreateAccountTypeRequest = createAccountTypeRequest };
            var result = await _mediator.Send(command);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateAccountTypeRequest updateAccountTypeRequest)
        {
            var command = new UpdateAccountType { UpdateAccountTypeRequest = updateAccountTypeRequest };
            var result = await _mediator.Send(command);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteAccountType { Id = id };
            var result = await _mediator.Send(command);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result);
        }

    }
}
