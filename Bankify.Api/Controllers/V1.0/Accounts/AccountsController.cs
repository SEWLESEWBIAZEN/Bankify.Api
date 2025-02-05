using Bankify.Application.Features.Queries.Accounts;
using Bankify.Domain.Models.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Bankify.Api.Controllers.V1._0.Accounts
{
    public class AccountsController : BaseController
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(RecordStatus? recordStatus)
        {
            var query = new GetAllAccounts{ RecordStatus = recordStatus };
            var result= await _mediator.Send(query);
            return result.IsError?HandleErrorResponse(result.Errors):Ok(result.Payload);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetAccountById { Id = id };
            var result = await _mediator.Send(query);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result.Payload);
        }
    }
}
