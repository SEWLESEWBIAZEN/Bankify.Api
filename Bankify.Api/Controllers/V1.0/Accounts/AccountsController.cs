using Bankify.Application.Common.DTOs.Accounts.Request;
using Bankify.Application.Common.DTOs.Accounts.Response;
using Bankify.Application.Features.Commands.Accounts;
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
            var accountList=_mapper.Map<List<AccountDetail>>(result.Payload);
            return result.IsError?HandleErrorResponse(result.Errors):Ok(accountList);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetAccountById { Id = id };
            var result = await _mediator.Send(query);
            var account = _mapper.Map<AccountDetail>(result.Payload);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(account);
        }

        [HttpGet("GetByUserId")]
        public async Task<IActionResult> GetByUserId(int userId)       
        {
            var query = new GetAccountsByUserId { UserId = userId };
            var result = await _mediator.Send(query);
            var accountList = _mapper.Map<List<AccountDetail>>(result.Payload);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(accountList);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateAccountRequest createAccountRequest)
        {
            var command = new CreateAccount { CreateAccountRequest = createAccountRequest };
            var result=await _mediator.Send(command);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result);
        }

        [HttpPut("Deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositToAccountRequest depositToAccountRequest)
        {
            var command = new DepositToAccount { DepositToAccountRequest = depositToAccountRequest };
            var result=await _mediator.Send(command);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(result);
        }

        [HttpGet("GenerateAcountNumber")]
        public async Task<IActionResult> GenerateAcountNumber()
        {
            var command = new GenerateAccountNumber();
            var result= await _mediator.Send(command);
            return result.IsError?HandleErrorResponse(result.Errors) : Ok(result.Payload);
        }
    }
}
