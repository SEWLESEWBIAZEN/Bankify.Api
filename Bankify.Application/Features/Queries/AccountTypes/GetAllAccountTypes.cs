using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bankify.Application.Features.Queries.AccountTypes
{
    public class GetAllAccountTypes:IRequest<OperationalResult<List<AccountType>>>
    {
        public RecordStatus? RecordStatus { get; set; }
    }

    internal class GetAllAccountTypesQueryHandler:IRequestHandler<GetAllAccountTypes, OperationalResult<List<AccountType>>>
    {
        private readonly IRepositoryBase<AccountType> _accountTypes;
        private readonly INetworkService _networkService;

        public GetAllAccountTypesQueryHandler(IRepositoryBase<AccountType> accountTypes,INetworkService networkService)
        {
            _accountTypes = accountTypes;
            _networkService = networkService;
        }

        public async Task<OperationalResult<List<AccountType>>> Handle(GetAllAccountTypes request, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<List<AccountType>>();
            try
            {
                var databaseAvailable = await _networkService.IsConnected();

                if (databaseAvailable)
                {
                    var accountTypes = await _accountTypes.Where(at => at.RecordStatus != RecordStatus.Deleted).ToListAsync();
                    if(accountTypes.Count==0)
                    {
                        result.AddError(ErrorCode.NotFound, "No Account Type Found!");
                    }
                    else
                    {
                        result.Payload = accountTypes;
                    }

            }
                else
            {
                result.AddError(ErrorCode.ServerError, "Network error(Database could not be reached)");
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
