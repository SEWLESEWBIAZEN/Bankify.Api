using Bankify.Application.Repository;
using Bankify.Domain.Models;
using Bankify.Domain.Models.Accounts;
using Bankify.Domain.Models.Transactions;
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
            builder.Services.AddScoped(typeof(IRepositoryBase<ATransaction>), typeof(RepositoryBase<ATransaction>));
            builder.Services.AddScoped(typeof(IRepositoryBase<TransactionEntry>), typeof(RepositoryBase<TransactionEntry>));
            builder.Services.AddScoped(typeof(IRepositoryBase<Transfer>), typeof(RepositoryBase<Transfer>));
            builder.Services.AddScoped(typeof(IRepositoryBase<AppClaim>), typeof(RepositoryBase<AppClaim>));
            builder.Services.AddScoped(typeof(IRepositoryBase<AppRole>), typeof(RepositoryBase<AppRole>));
            builder.Services.AddScoped(typeof(IRepositoryBase<UserRole>), typeof(RepositoryBase<UserRole>)); 
            builder.Services.AddScoped(typeof(IRepositoryBase<RoleClaim>), typeof(RepositoryBase<RoleClaim>));

        }
    }
}
