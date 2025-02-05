using Bankify.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Quartz;
using System.Text.Json.Serialization;

namespace Bankify.Api.Registrars
{
    public class MvcRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddMvc(setupAction: options =>
            {
                options.Filters.Add(typeof(AuthorizationHandler));
                options.EnableEndpointRouting = false;
            });

            builder.Services.AddControllers(config =>
            {
                config.Filters.Add(typeof(ExceptionHandler));
            })
                .AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
            //builder.Services.AddQuartz(q =>
            //{
            //    q.UseMicrosoftDependencyInjectionJobFactory();

            //    q.ScheduleJob<IReminderJob>(trigger => trigger
            //        .WithIdentity("ReminderJob-trigger")
            //        .WithCronSchedule("0 */3 * * * ?")); // run every 3 minutes  
            //});

            //builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            builder.Services.AddVersionedApiExplorer(config =>
            {
                config.GroupNameFormat = "'v'VVV";
                config.SubstituteApiVersionInUrl = true;

            });

            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("localhost:3000",
                                                          "svhqiis03:1111",
                                                          "https://legaloffice.ethiopianairlines.com",
                                                          "http://localhost:3000",
                                                          "http://svhqdts01:1180")
                                                          .AllowAnyHeader()
                                                          .AllowAnyMethod()
                                                          .AllowAnyOrigin();
                                  });
            });

            builder.Services.BuildServiceProvider();
            builder.Services.AddEndpointsApiExplorer();
           // builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));
            //builder.Services.Configure<ServicesUrl>(builder.Configuration.GetSection("ServicesUrl"));
        }
    }
}
