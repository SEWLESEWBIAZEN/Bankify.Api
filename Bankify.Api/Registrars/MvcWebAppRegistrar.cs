using Microsoft.AspNetCore.Mvc.ApiExplorer;
using QuestPDF.Infrastructure;

namespace Bankify.Api.Registrars
{
    public class MvcWebAppRegistrar : IWebApplicationRegistrar
    {
        public void RegisterPipelineComponents(WebApplication app)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"{description.GroupName}/swagger.json", description.ApiVersion.ToString());
                }
            });

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Cache-control", "no-store");
                context.Response.Headers.Add("Pragma", "no-cache");

                await next();
            });

            app.UseCookiePolicy(
                  new CookiePolicyOptions
                  {
                      Secure = CookieSecurePolicy.Always
                  }
            );            
            app.MapGet("/", () => "Bankify - API");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MigrateDatabase();
        }
    }
}
