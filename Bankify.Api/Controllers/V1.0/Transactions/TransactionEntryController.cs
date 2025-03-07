using Bankify.Application.Common.DTOs.TransactionEntries.Response;
using Bankify.Application.Features.Queries.TransactionEntries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bankify.Api.Controllers.V1._0.Transactions
{
    public class TransactionEntryController : BaseController
    {
        [Authorize]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {         
            var query = new GetAllTransactionEntries();
            var result = await _mediator.Send(query);
            var transactionEntriesList = _mapper.Map<List<TransactionEntryDetail>>(result.Payload);
            return result.IsError?HandleErrorResponse(result.Errors):Ok(transactionEntriesList);
        }

        [Authorize]
        [HttpGet("GetByTransaction")]
        public async Task<IActionResult> GetByTransaction(int transactionId)
        {
            var query = new GetTransactionEntriesByTransaction { TransactionId=transactionId};
            var result=await _mediator.Send(query);
            var transactionEntries = _mapper.Map<List<TransactionEntryDetail>>(result.Payload);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(transactionEntries);
        }
    }
}
