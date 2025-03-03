using Bankify.Application.Common.DTOs.AccountTypes.Request;
using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Bankify.Application.Features.Commands.AccountTypes
{
    public class CreateAccountType:IRequest<OperationalResult<AccountType>>
    {
        public CreateAccountTypeRequest CreateAccountTypeRequest { get; set; }
    }
    internal class CreateAccountTypeCommandHandler:IRequestHandler<CreateAccountType, OperationalResult<AccountType>>
    {
        private readonly IRepositoryBase<AccountType> _accountType;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IActionLoggerService _actionLoggerService;
        private ISession session;

        public CreateAccountTypeCommandHandler(IRepositoryBase<AccountType> accountType, INetworkService networkService, IHttpContextAccessor contextAccessor, IActionLoggerService actionLoggerService)
        {
            _accountType = accountType;
            _networkService = networkService;
            _contextAccessor = contextAccessor;
            session = _contextAccessor.HttpContext.Session;
            _actionLoggerService = actionLoggerService;
        }

        public async Task<OperationalResult<AccountType>> Handle(CreateAccountType createAccountTypeRequest, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<AccountType>();
            var request= createAccountTypeRequest.CreateAccountTypeRequest;
            var sessionUser = session.GetString("user");
            try {
                var dbAvailable = await _networkService.IsConnected();
                if (!dbAvailable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error(Unable to reach to database)");
                    return result;
                }
                if (request == null)
                {
                    result.AddError(ErrorCode.EmptyRquest, "Empty Request has Sent");
                    return result;
                }
                if( _accountType.ExistWhere(at => at.Name == request.Name))
                {
                    result.AddError(ErrorCode.RecordExists, $"{request.Name} account type already exists!");
                    return result;
                }
               
                if (dbAvailable) 
                {
                    var newAccountType=new AccountType();
                    newAccountType.Name=request.Name;
                    newAccountType.Description = request.Description;
                    newAccountType.UniqueId= request.UniqueId;
                    newAccountType.InterestRate=request.InterestRate;
                    //update audit
                    newAccountType.Register(sessionUser);
                    await _accountType.AddAsync(newAccountType);                   
                    await _actionLoggerService.TakeActionLog(ActionType.Create,"Account Type",newAccountType.Id,  sessionUser,$"New Account Type namely: %{newAccountType.Name}% was created on {DateTime.Now} by {sessionUser}" );
                    result.Message = "New Account Type Added!";
                }
                else 
                { 
                    result.AddError(ErrorCode.ServerError, "Network Error");
                }
            } catch (Exception ex) {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
    }
}
