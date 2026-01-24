using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Shared;

namespace Bankify.Application.Services
{
    public interface IAccountRetrievalService
    {
       Task<OperationalResult<Account>> GetAccount(string accountNumber);
    }

    public class AccountRetrievalService : IAccountRetrievalService
    {
        private IRepositoryBase<Account> _accounts;
        private INetworkService _networkService;
        public AccountRetrievalService(IRepositoryBase<Account> accounts, INetworkService networkService)
        {
            _accounts = accounts;
            _networkService = networkService;
        }

        public async Task<OperationalResult<Account>> GetAccount(string accountNumber)
        {
            var result = new OperationalResult< Account>();

            try 
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable) 
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error (unable to reach database)");
                    return result;                    
                }
                var account = await _accounts.FirstOrDefaultAsync(a => a.RecordStatus != RecordStatus.Deleted && a.AccountNumber == accountNumber);
                if(account is null)
                {
                    result.AddError(ErrorCode.NotFound, "Account Not Found");
                    return result ;
                }
                result.Payload=account;
            }
            catch (Exception ex) 
            {
                result.AddError(ErrorCode.ServerError,ex.Message);
            }

            return result;

        }
    }
}
