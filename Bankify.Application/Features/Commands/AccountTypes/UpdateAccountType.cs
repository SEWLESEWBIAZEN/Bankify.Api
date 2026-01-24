using Bankify.Application.Common.DTOs.AccountTypes.Request;
using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Graph.Models;

namespace Bankify.Application.Features.Commands.AccountTypes
{
    public class UpdateAccountType:IRequest<OperationalResult<AccountType>>
    {
        public UpdateAccountTypeRequest UpdateAccountTypeRequest { get; set; }
    }
    internal class UpdateAccountTypeCommandHandler : IRequestHandler<UpdateAccountType, OperationalResult<AccountType>>
    {
        private readonly IRepositoryBase<AccountType> _accountType;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IActionLoggerService _actionLoggerService;

        private ISession session;
        public UpdateAccountTypeCommandHandler(IRepositoryBase<AccountType> accountType, INetworkService networkService, IHttpContextAccessor contextAccessor, IActionLoggerService actionLoggerService)
        {
            _accountType = accountType;
            _networkService = networkService;
            _contextAccessor = contextAccessor;
            session = _contextAccessor.HttpContext.Session;
            _actionLoggerService = actionLoggerService;
        }

        public async Task<OperationalResult<AccountType>> Handle(UpdateAccountType updateAccountType, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<AccountType>();
            var request=updateAccountType.UpdateAccountTypeRequest;
            var sessionUser = session.GetString("user");
            try 
            {
                if(request.Id==0)
                {
                    result.AddError(ErrorCode.EmptyRquest, "Empty Account Type Id has Sent");
                    return result;
                }
                if(await _networkService.IsConnected())
                {
                    var accountTypeToBeUpdated=await _accountType.FirstOrDefaultAsync(ac=>ac.Id==request.Id && ac.RecordStatus!=RecordStatus.Deleted);
                    if (accountTypeToBeUpdated != null) 
                    {
                        accountTypeToBeUpdated.Name = request.Name;
                        accountTypeToBeUpdated.UniqueId = request.UniqueId;
                        accountTypeToBeUpdated.Description= request.Description;
                        accountTypeToBeUpdated.InterestRate=request.InterestRate;

                        accountTypeToBeUpdated.UpdateAudit(sessionUser);
                        await _accountType.UpdateAsync(accountTypeToBeUpdated);
                        await _actionLoggerService.TakeActionLog(ActionType.Update, "Account Type", accountTypeToBeUpdated.Id, sessionUser, $"Account Type namely: %{accountTypeToBeUpdated.Name}% was Updated on {DateTime.Now} by {sessionUser}");
                        result.Message = "Account Type Updated";
                    }
                    else
                    {
                        result.AddError(ErrorCode.NotFound, "Desired Account Type is not Found!");
                    }

                }
                else
                {
                    result.AddError(ErrorCode.ServerError, "Network Error(Database not Reachable)");
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
