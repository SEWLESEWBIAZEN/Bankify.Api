using Bankify.Application.Common.DTOs.Transactions.Response;
using Bankify.Application.Features.Queries.Transactions;
using Bankify.Domain.Models.Shared;
using Microsoft.AspNetCore.Mvc;
namespace Bankify.Api.Controllers.V1._0.Transactions
{
    public class TransactionsController : BaseController
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllTransactions();
            var result = await _mediator.Send(query);
            var transactionList = _mapper.Map<List<TransactionDetail>>(result.Payload);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(transactionList);
        }

        //[HttpGet("GetByType")]
        //public async Task<IActionResult> GetByType(int transactionType)
        //{
        //    var query = new GetTransactionsByType { TransactionType = transactionType };
        //    var result = await _mediator.Send(query);
        //    var transactionList = _mapper.Map<List<TransactionDetail>>(result.Payload);
        //    return result.IsError ? HandleErrorResponse(result.Errors) : Ok(transactionList);
        //}

        //by account
        //[HttpGet("GetByAccount")]
        //public async Task<IActionResult> GetByAccount(string accountNumber)
        //{
        //    var query = new GetTransactionsByAccount { AccountNumber = accountNumber };
        //    var result = await _mediator.Send(query);
        //    var transactionList = _mapper.Map<List<TransactionDetail>>(result.Payload);
        //    return result.IsError ? HandleErrorResponse(result.Errors) : Ok(transactionList);
        //}

        //by id       
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetTransactionById { Id = id };
            var result = await _mediator.Send(query);
            var transaction= _mapper.Map<TransactionDetail>(result.Payload);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(transaction);
        }
        //by status
        [HttpGet("GetByStatus")]
        public async Task<IActionResult> GetByStatus(TransactionStatus transactionStatus)
        {
            var query = new GetTransactionsByStatus { TransactionStatus = transactionStatus };
            var result = await _mediator.Send(query);
            var transactionList = _mapper.Map<List<TransactionDetail>>(result.Payload);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(transactionList);
        }
    }
}
