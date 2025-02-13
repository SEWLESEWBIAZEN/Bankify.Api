using Bankify.Application.Common.Helpers;
using MediatR;

namespace Bankify.Application.Features.Commands.Accounts
{
    public class DepositToAccount:IRequest<OperationalResult<DepositReciept>>
    {
    }
    public class DepositReciept
    {
         
    }
}
