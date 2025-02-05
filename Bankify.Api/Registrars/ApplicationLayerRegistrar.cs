namespace Bankify.Api.Registrars
{
    public class ApplicationLayerRegistrar
    {
    }
    namespace Banking.Api.Registrars
    {
        public class ApplicationLayerRegistrar : IWebApplicationBuilderRegistrar
        {
            public void RegisterServices(WebApplicationBuilder builder)
            {
                builder.Services.AddDistributedMemoryCache();
                builder.Services.AddSession(options =>
                {
                    options.IOTimeout = TimeSpan.FromMinutes(10);
                });
            }
        }
    }

}
