using Bankify.Api.Registrars;
using Bankify.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace Lms.Api.Registrars
{
    public class DbRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            var connectionString = Environment.GetEnvironmentVariable("MSSQLDB_HOST");
#if DEBUG
            connectionString = string.Empty;
#endif
            if (string.IsNullOrEmpty(connectionString))
                connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<BankifyDbContext>(options => options.UseSqlServer(connectionString));
           //builder.Services.Configure<ServicesUrl>(builder.Configuration.GetSection("ServicesUrl"));
           //builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));
           //builder.Services.Configure<SftpSettings>(builder.Configuration.GetSection("SftpSettings"));
        }
    }
}
