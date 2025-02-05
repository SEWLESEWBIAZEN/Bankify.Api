namespace Bankify.Api.Registrars
{
    public class ApplicationInsightsRegistrar : IWebApplicationBuilderRegistrar
    {
        [Obsolete]
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPINSIGHTS_CONNECTIONSTRING"]);

        }
    }
}
