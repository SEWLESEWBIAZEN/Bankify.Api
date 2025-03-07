using Bankify.Application.Common.Helpers.IHttpService;
using Bankify.Application.Services;
using Lms.Application.Common.Helpers.HttpService;
using MediatR;

namespace Bankify.Api.Registrars
{
    public class ServicesRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddHttpContextAccessor();
            //builder.Services.AddScoped<IIdentityService, IdentityService>();
            builder.Services.AddScoped<IRegisterTransactionEntriesService, RegisterTransactionEntriesService>();
            builder.Services.AddScoped<IHttpService, HttpService>();
            builder.Services.AddSingleton<IFileStorageService, FileStorageService>();
            builder.Services.AddScoped<INetworkService, NetworkService>();
            builder.Services.AddScoped<IActionLoggerService, ActionLoggerService>();
            //builder.Services.AddScoped<IEmailSender, EmailSenderServices>();
            builder.Services.AddScoped<IAccountRetrievalService, AccountRetrievalService>();         
            
            builder.Services.AddHttpClient();

        }
    }
}
