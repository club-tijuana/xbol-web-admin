using Odasoft.XBOL.AdminPortal.Components;
using Odasoft.XBOL.AdminPortal.Extensions;
using Odasoft.XBOL.AdminPortal.Schema;
using Odasoft.XBOL.Business.Extensions;

if (args.Contains("--generate-schema"))
{
    var outputPath = Path.GetFullPath(
        Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "appsettings.schema.json"));
    AppSettingsSchemaGenerator.GenerateAndWrite(outputPath);
    return;
}

var builder = WebApplication.CreateBuilder(args);

// Infrastructure
builder.Services.ConfigureOptions(builder.Configuration);

// Blazor framework
builder.Services.ConfigureBlazor();
builder.Services.AddHealthChecks();

// Security
builder.Services.ConfigureAuthentication();

// Localization
builder.Services.ConfigureLocalization();

// Application
builder.Services.ConfigureServices();
builder.Services.ConfigureApplicationServices();

// External clients
builder.Services.ConfigureHttpClients();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

// Only use HTTPS redirection when running directly (Visual Studio, dotnet run)
// Containers handle TLS at load balancer/reverse proxy level
if (!app.Environment.IsProduction()
    || string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")))
{
    app.UseHttpsRedirection();
}

app.UseRequestLocalization();
app.UseHsts();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.UseAuthentication();
app.UseAuthorization();

// Map health check endpoint for container health monitoring
app.MapHealthChecks("/healthz", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var appName = app.Environment.ApplicationName ?? "unknown";
        var environment = app.Environment.EnvironmentName ?? "unknown";
        var dockerImageVersion = Environment.GetEnvironmentVariable("DOCKER_IMAGE_VERSION") ?? "unknown";
        var response = new
        {
            appName,
            environment,
            status = report.Status.ToString(),
            dockerImageVersion
        };
        await context.Response.WriteAsJsonAsync(response);
    }
});

app.Run();
