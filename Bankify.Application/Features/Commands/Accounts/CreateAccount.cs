using Bankify.Application.Common.DTOs.Accounts.Request;
using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;

namespace Bankify.Application.Features.Commands.Accounts
{
    public class CreateAccount:IRequest<OperationalResult<Account>>
    {
        public CreateAccountRequest CreateAccountRequest { get; set; }
    }
    internal class CreateAccountCommandHandler:IRequestHandler<CreateAccount, OperationalResult<Account>>
    {
        private readonly IRepositoryBase<Account> _accounts;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IActionLoggerService _actionLoggerService;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        private ISession session;
        public CreateAccountCommandHandler(IRepositoryBase<Account> accounts, INetworkService networkService, IHttpContextAccessor contextAccessor, IActionLoggerService actionLoggerService, IConfiguration configuration, HttpClient httpClient)
        {
            _accounts = accounts;
            _networkService = networkService;
            _contextAccessor = contextAccessor;
            _actionLoggerService = actionLoggerService;
            session = _contextAccessor.HttpContext.Session;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<OperationalResult<Account>> Handle(CreateAccount createAccount, CancellationToken cancellationToken)
        {
            var sessionUser = session.GetString("user");
            var result=new OperationalResult<Account>();
            var request=createAccount.CreateAccountRequest;
            try {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable) 
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach database)");
                    return result;
                }
                //generate account number from the db
                var baseUrl = _configuration["SftpSettings:BaseUrl"];
                var requestUrl = $"{baseUrl}/api/v1/Accounts/GenerateAcountNumber";
                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                var payload = await response.Content.ReadFromJsonAsync<GenerateAccountNumberResponse>();

                var accountExist = await _accounts.ExistWhereAsync(a => a.AccountNumber == payload.AccountNumber && a.UserId == request.UserId);
                if (accountExist) 
                {
                    result.AddError(ErrorCode.RecordExists, "Account already Existed");
                    return result;
                }             

                //creating new account object
                var newAccount = new Account
                {
                    AccountNumber= payload.AccountNumber,
                    UserId= request.UserId,
                    Balance= request.Balance,
                    AccountTypeId= request.AccountTypeId
                };

                await _accounts.AddAsync(newAccount);
                await _actionLoggerService.TakeActionLog(ActionType.Create,"Account", newAccount.Id, sessionUser, $"New Account with Number: {newAccount.AccountNumber} was created at {DateTime.Now} by {sessionUser}");
                result.Message = "New Account Created";
                result.Payload= newAccount;
                return result;
            }
            catch (Exception ex) 
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
                return result;
            }            
        }
    } 
}
