using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bankify.Application.Features.Commands.Accounts
{
    public class GenerateAccountNumber:IRequest<OperationalResult<GenerateAccountNumberResponse>>
    {
    }
    internal class GenerateAccountNumberCommandHandler:IRequestHandler<GenerateAccountNumber, OperationalResult<GenerateAccountNumberResponse>>
    {
        private readonly IRepositoryBase<Account> _accounts;
        private readonly INetworkService _networkService;

        public GenerateAccountNumberCommandHandler(IRepositoryBase<Account> accounts, INetworkService networkService)
        {
            _accounts = accounts;
            _networkService = networkService;
        }

        public async Task<OperationalResult<GenerateAccountNumberResponse>> Handle(GenerateAccountNumber generateAccountNumber, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<GenerateAccountNumberResponse>();
            var dbReachable = await _networkService.IsConnected();
            var newAccountNumber = 1000000000000;
            try
            {
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach Database)");
                    return result;
                }
                var accounts = await _accounts.Where(a => a.RecordStatus != RecordStatus.Deleted).OrderByDescending(a => a.AccountNumber).ToListAsync();
                if (accounts.Count != 0)
                {
                    var latestAccount = accounts.FirstOrDefault();
                    newAccountNumber = Convert.ToInt64(latestAccount.AccountNumber) + 1;
                    result.Payload = new GenerateAccountNumberResponse { AccountNumber = newAccountNumber.ToString() };                  
                    return result;
                }
                result.Payload = new GenerateAccountNumberResponse { AccountNumber = newAccountNumber.ToString() };
                return result;
            }
            catch (Exception ex) 
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
                return result;
            }           
            
            
       }



    }

    public class GenerateAccountNumberResponse
    {
        public string AccountNumber { get; set; }
    }
}
