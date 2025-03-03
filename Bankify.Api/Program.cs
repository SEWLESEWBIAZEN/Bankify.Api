using Bankify.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.RegisterServices(typeof(Program));

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var app = builder.Build();
app.RegisterPipelineComponents(typeof(Program));

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; connect-src 'self' ms-word:;");
    await next();
});

app.UseSession();
app.UseCors(MyAllowSpecificOrigins);
app.Run();

