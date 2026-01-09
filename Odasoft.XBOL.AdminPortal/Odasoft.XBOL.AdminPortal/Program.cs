using MudBlazor.Services;
using MudBlazor.Translations;
using Odasoft.XBOL.AdminPortal;
using Odasoft.XBOL.AdminPortal.Components;
using Odasoft.XBOL.AdminPortal.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.TopCenter;
    config.SnackbarConfiguration.SnackbarVariant = MudBlazor.Variant.Text;
    config.SnackbarConfiguration.MaxDisplayedSnackbars = 5;
});

builder.Services.AddHttpClient<IAdminApiClient, AdminApiClient>(
    (provider, client) =>
    {
        client.BaseAddress = new Uri(
            builder.Configuration.GetValue("AdminApiClientBaseAddress", "https://localhost:7014/")
        );
    }
);
builder.Services.AddHealthChecks();

// Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    string[] supportedCultures = ["es"];
    options.SetDefaultCulture("es");
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});
builder.Services.AddMudTranslations();

// Services
builder.Services.AddScoped<IEventService, MockEventService>();

// TODO: Replace with actual event service
// builder.Services.AddHttpClient<IEventService, HttpEventService>(client =>
// {
//     client.BaseAddress = new Uri("https://api.xbol.com/");
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

// Only use HTTPS redirection when running directly (Visual Studio, dotnet run)
// Containers handle TLS at load balancer/reverse proxy level
if (
    !app.Environment.IsProduction()
    || string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"))
)
{
    app.UseHttpsRedirection();
}

app.UseRequestLocalization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// Map health check endpoint for container health monitoring
app.MapHealthChecks("/healthz");

app.Run();
