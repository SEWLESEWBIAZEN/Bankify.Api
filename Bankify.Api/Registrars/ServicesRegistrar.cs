using Bankify.Application.Common.Helpers.HttpService;
using Bankify.Application.Common.Helpers.IHttpService;
using Bankify.Application.Services;
using Lms.Application.Common.Helpers.HttpService;
using MediatR;
using System.ComponentModel.Design;

namespace Bankify.Api.Registrars
{
    public class ServicesRegistrar:IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddHttpContextAccessor();
            //builder.Services.AddScoped<IIdentityService, IdentityService>();
            //builder.Services.AddScoped<IUserService, UsersService>();
            builder.Services.AddScoped<IHttpService, HttpService>();
            builder.Services.AddSingleton<IFileStorageService, FileStorageService>();
            //builder.Services.AddScoped<IContractLoggerService, ContractLoggerService>();
            //builder.Services.AddScoped<ICaseActivityLoggerService, CaseActivityLogger>();
            //builder.Services.AddScoped<IEmailSender, EmailSenderServices>();
            //builder.Services.AddScoped<ISharePointService, SharePointService>();
            //builder.Services.AddScoped<IFileConvertor, FileConvertor>();
            //builder.Services.AddScoped<IMatterLoggerService, MatterLoggerService>();
            //builder.Services.AddScoped<ITaskLoggerService, TaskLoggerService>();
            builder.Services.AddHttpClient();

        }
    }
}
