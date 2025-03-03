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
    public class DeleteAccountType:IRequest<OperationalResult<AccountType>>
    {
        public int Id { get; set; }
    }
    internal class DeleteAccountTypeCommandHandler:IRequestHandler<DeleteAccountType, OperationalResult<AccountType>>
    {
        private readonly IRepositoryBase<AccountType> _accountTypes;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActionLoggerService _actionLoggerService;

        private ISession session;

        public DeleteAccountTypeCommandHandler(IRepositoryBase<AccountType> accountTypes, INetworkService networkService, IHttpContextAccessor httpContextAccessor, IActionLoggerService actionLoggerService)
        {
            _accountTypes = accountTypes;
            _networkService = networkService;
            _httpContextAccessor = httpContextAccessor;

            session = _httpContextAccessor.HttpContext.Session;
            _actionLoggerService = actionLoggerService;
        }

        public async Task<OperationalResult<AccountType>> Handle(DeleteAccountType request, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<AccountType>();
            var sessionUser = session.GetString("user");
            try
            {
                if (request.Id == 0)
                {
                    result.AddError(ErrorCode.EmptyRquest, "Empty Id has Sent");
                    return result;
                }
                if(await _networkService.IsConnected())
                {
                    var accountTypeToBeDeleted=await _accountTypes.FirstOrDefaultAsync(at=>at.Id == request.Id && at.RecordStatus!=RecordStatus.Deleted);
                    if (accountTypeToBeDeleted==null) 
                    {
                        result.AddError(ErrorCode.NotFound, "Account Type doesn't exist");

                    }
                    else
                    {
                        accountTypeToBeDeleted.RecordStatus=RecordStatus.Deleted;

                        accountTypeToBeDeleted.UpdateAudit(sessionUser);
                        await _accountTypes.UpdateAsync(accountTypeToBeDeleted);
                        await _actionLoggerService.TakeActionLog(ActionType.Delete, "Account Type", accountTypeToBeDeleted.Id, sessionUser, $"Account Type namely: %{accountTypeToBeDeleted.Name}% was deleted on {DateTime.Now} by {sessionUser}");
                        result.Message = "Account Type has deleted";
                    }
                }
                else
                {
                    result.AddError(ErrorCode.ServerError, "Network Error(Database can not be reached");
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
