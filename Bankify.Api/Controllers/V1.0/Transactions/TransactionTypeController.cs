using Bankify.Application.Common.DTOs.TransactionTypes.Request;
using Bankify.Application.Features.Commands.TransactionTypes;
using Bankify.Application.Features.Queries.TransactionTypes;
using Microsoft.AspNetCore.Mvc;

namespace Bankify.Api.Controllers.V1._0.Transactions
{
    public class TransactionTypeController : BaseController
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllTransactionTypes();
            var result = await _mediator.Send(query);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result.Payload);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetTransactionTypeById { Id=id};
            var result = await _mediator.Send(query);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateTransactionTypeRequest createTransactionTypeRequest)
        {
            var command = new CreateTransactionType { CreateTransactionTypeRequest = createTransactionTypeRequest };
            var result=await _mediator.Send(command);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateTransactionTypeRequest updateTransactionTypeRequest)
        {
            var command = new UpdateTransactionType { UpdateTransactionTypeRequest = updateTransactionTypeRequest };
            var result = await _mediator.Send(command);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result);
        }

    }
}
