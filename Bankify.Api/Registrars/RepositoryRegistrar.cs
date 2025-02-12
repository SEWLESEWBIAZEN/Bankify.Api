using Bankify.Application.Repository;
using Bankify.Domain.Models;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Users;

namespace Bankify.Api.Registrars
{
    public class RepositoryRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {

            builder.Services.AddScoped(typeof(IRepositoryBase<BUser>), typeof(RepositoryBase<BUser>));
            builder.Services.AddScoped(typeof(IRepositoryBase<Account>), typeof(RepositoryBase<Account>));
            builder.Services.AddScoped(typeof(IRepositoryBase<AccountType>), typeof(RepositoryBase<AccountType>));
            builder.Services.AddScoped(typeof(IRepositoryBase<ActionLog>), typeof(RepositoryBase<ActionLog>));

        }
    }
}
