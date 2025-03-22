using Bankify.Application.Common.DTOs.Accounts.Request;
using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Transactions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Bankify.Application.Features.Commands.Accounts
{
    public class TransferFromAccountToAccount:IRequest<OperationalResult<Transfer>>
    {
        public TransferFromAccountToAccountRequest TransferFromAccountToAccountRequest { get; set; }=new TransferFromAccountToAccountRequest();
    }

    internal class TransferFromAccountToAccountCommandHandler:IRequestHandler<TransferFromAccountToAccount, OperationalResult<Transfer>>
    {
        private readonly IRepositoryBase<Transfer> _transfers;
        private readonly IRepositoryBase<ATransaction> _transactions;
        private readonly IRepositoryBase<Account> _accounts;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private ISession session;

        public TransferFromAccountToAccountCommandHandler(IRepositoryBase<Transfer> transfers, IRepositoryBase<ATransaction> transactions, IRepositoryBase<Account> accounts, INetworkService networkService, IHttpContextAccessor httpContextAccessor)
        {
            _transfers = transfers;
            _transactions = transactions;
            _accounts = accounts;
            _networkService = networkService;
            _httpContextAccessor = httpContextAccessor;

            session=_httpContextAccessor.HttpContext.Session;
        }

        public async Task<OperationalResult<Transfer>> Handle(TransferFromAccountToAccount command, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<Transfer>();
            var sessionUser = session.GetString("user");
            var request=command.TransferFromAccountToAccountRequest;
            try 
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable) 
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error (Unable to reach database)");
                    return result;
                }
                if(request.AmmountTransfered<=0.00m || request.TransferredFromAccountNumber is null || request.TransferredToAccountNumber is null)
                {
                    result.AddError(ErrorCode.ValidationError, "Invalid Data Sent");
                    return result;
                }
                //both accounts are different
                if(request.TransferredFromAccountNumber== request.TransferredToAccountNumber)
                {
                    result.AddError(ErrorCode.ValidationError, "Both Accounts are the same");
                    return result;
                }
                    //retrieve accounts i.e debit and credit
                var accountToBeCredited = await _accounts.FirstOrDefaultAsync(a => a.AccountNumber == request.TransferredToAccountNumber);
                var accountToBeDebited=await _accounts.FirstOrDefaultAsync(a=>a.AccountNumber== request.TransferredFromAccountNumber);

                if (accountToBeCredited is null )
                {
                    result.AddError(ErrorCode.NotFound, $"{request.TransferredToAccountNumber} is Not Found");
                    return result;
                }

                if (accountToBeDebited is null)
                {
                    result.AddError(ErrorCode.NotFound, $"{request.TransferredFromAccountNumber} is Not Found");
                    return result;
                }

                //check if debit account  has sufficient balance
                if (accountToBeDebited.Balance <= request.AmmountTransfered)
                {
                    result.AddError(ErrorCode.ValidationError, $"{accountToBeDebited.AccountNumber} has no sufficient balance to transfer");
                    return result;
                }

                //save both accounts previous balances to temp variables
                var accountToBeCreditedBalanceBefore = accountToBeCredited.Balance;
                var accountToBeDebitedBalanceBefore = accountToBeDebited.Balance;

                //update balances
                accountToBeDebited.Balance=accountToBeDebited.Balance- request.AmmountTransfered;
                accountToBeCredited.Balance = accountToBeCredited.Balance + request.AmmountTransfered;

                //update audit
                accountToBeCredited.UpdateAudit(sessionUser);
                accountToBeDebited.UpdateAudit(sessionUser);
               
                //update accounts
               var transferSuccess= await _accounts.UpdateRangeAsync([accountToBeDebited, accountToBeCredited]);

                //register each operation as transaction with type debit and credit
                var debitTransaction = new ATransaction
                {
                    Description="Transfer",
                
                    Status = TransactionStatus.Completed,
                    TransactionDate = DateTime.Now

                };
                debitTransaction.Register(sessionUser);
                var creditTransaction = new ATransaction
                {
                    Description = "Transfer",
                   
                    Status = TransactionStatus.Completed,
                    TransactionDate = DateTime.Now
                };

                creditTransaction.Register(sessionUser);
                if (transferSuccess)
                {
                    await _transactions.AddRangeAsync([debitTransaction, creditTransaction]);

                    //if transfer is successful register transfer
                    var newTransfer = new Transfer
                    {
                        TransferedFromId=accountToBeDebited.Id,
                        TransferredToId=accountToBeCredited.Id,
                        AmmountTransfered=request.AmmountTransfered,
                        TransferStatus=TransactionStatus.Completed,
                        TransferedOn=DateTime.Now
                    };
                    await _transfers.AddAsync(newTransfer);
                    result.Payload = newTransfer;
                    result.Message = $" 'ETB{newTransfer.AmmountTransfered}' Transferred '{accountToBeDebited.AccountNumber}' to '{accountToBeCredited.AccountNumber}' Successfully";
                }                
               
            }
            catch (Exception ex) 
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
    }
}
