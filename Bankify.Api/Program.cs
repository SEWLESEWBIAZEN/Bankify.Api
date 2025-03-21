using Bankify.Api.Extensions;
using Microsoft.Extensions.FileProviders;


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

// Serve static files from the "Uploads" folder
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Uploads"
});

app.UseSession();
app.UseCors(MyAllowSpecificOrigins);
app.Run();

