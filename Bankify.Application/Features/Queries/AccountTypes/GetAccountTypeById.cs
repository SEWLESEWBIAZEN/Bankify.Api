using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Bankify.Application.Features.Queries.AccountTypes
{
    public class GetAccountTypeById:IRequest<OperationalResult<AccountType>>
    {
        public int Id { get; set; }
    }
    internal class GetAccountTypeByIdQueryHandler : IRequestHandler<GetAccountTypeById, OperationalResult<AccountType>>
    {
        private readonly IRepositoryBase<AccountType> _accountTypes;
        private readonly INetworkService _networkService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        private ISession _session=>_httpContextAccessor.HttpContext.Session;

        public GetAccountTypeByIdQueryHandler(IRepositoryBase<AccountType> accountTypes, INetworkService networkService, IHttpContextAccessor httpContextAccessor)
        {
            _accountTypes = accountTypes;
            _networkService = networkService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OperationalResult<AccountType>> Handle(GetAccountTypeById request, CancellationToken cancellationToken)
        {
            var user=_session.GetString("user");
            var result= new OperationalResult<AccountType>();
            try 
            {
                if (request.Id == 0 || request?.Id == null) 
                {
                    result.AddError(ErrorCode.EmptyRquest, "Empty Id Sent!");
                    return result;
                }
                var dbAvailable = await _networkService.IsConnected();
                if (dbAvailable) 
                {
                    var accountType=await _accountTypes.FirstOrDefaultAsync(at=>at.Id==request.Id && at.RecordStatus!=RecordStatus.Deleted);
                    if (accountType == null)
                    {
                        result.AddError(ErrorCode.NotFound, "Account Type Not Found or deleted!");
                    }
                    else
                    {
                        result.Payload = accountType;
                    }
                }
                else
                {
                    result.AddError(ErrorCode.ServerError, "Network Error(Database is not accessible)");
                }
            }
            catch(Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
            

  }
}
