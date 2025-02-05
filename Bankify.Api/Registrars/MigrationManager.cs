
using Bankify.Infrastructure.Context;

namespace Bankify.Api.Registrars
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<BankifyDbContext>())
                {
                    try
                    {
                        //appContext.Database.Migrate();
                        appContext.Database.EnsureCreated();
                    }
                    catch (Exception)
                    {
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
            }
            return webApp;
        }
    }
}
