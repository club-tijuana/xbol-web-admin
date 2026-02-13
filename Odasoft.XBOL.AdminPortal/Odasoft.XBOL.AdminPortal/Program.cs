using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.Options;
using MudBlazor.Services;
using MudBlazor.Translations;
using Odasoft.XBOL.AdminPortal.Components;
using Odasoft.XBOL.AdminPortal.Configs;
using Odasoft.XBOL.AdminPortal.Services;
using Odasoft.XBOL.AdminPortal.Services.Contracts;
using Odasoft.XBOL.AdminPortal.States;
using Odasoft.XBOL.Business;
using Odasoft.XBOL.Business.Extensions;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

#region AppSettings
Authentication authenticationConfig = builder.Configuration.GetSection("Authentication").Get<Authentication>()!;
#endregion

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.TopCenter;
    config.SnackbarConfiguration.SnackbarVariant = MudBlazor.Variant.Text;
    config.SnackbarConfiguration.MaxDisplayedSnackbars = 5;
});

builder.Services.AddHealthChecks();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    string[] supportedCultures = ["es-MX"];
    options.SetDefaultCulture("es-MX");
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});

builder.Services.Configure<CircuitOptions>(options =>
{
    options.DetailedErrors = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/login";
});

var mexicoCulture = new CultureInfo("es-MX");
CultureInfo.DefaultThreadCurrentCulture = mexicoCulture;
CultureInfo.DefaultThreadCurrentUICulture = mexicoCulture;
builder.Services.AddMudTranslations();
builder.Services.AddAuthorization();
builder.Services.AddAuthorizationCore();

// Services
builder.Services.ConfigureServices();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEventService, ApiEventService>();
builder.Services.AddScoped<GeneralService>();
builder.Services.AddScoped<ISeasonPassService, SeasonPassService>();
builder.Services.AddScoped<ISeasonService, SeasonService>();

builder.Services.AddScoped<CartState>();
builder.Services.AddScoped<LoadingState>();

builder.Services.AddHttpClient<IAdminClient, AdminClient>(
    (provider, client) =>
    {
        var config = provider.GetRequiredService<IOptions<AdminApiClientConfig>>().Value;
        client.BaseAddress = new Uri(config.BaseAddress);
        client.DefaultRequestHeaders.Add("Accept-Language", "es");
    });

builder.Services.AddOptions<Authentication>()
    .BindConfiguration("Authentication");

builder.Services.AddOptions<AdminApiClientConfig>()
    .BindConfiguration("AdminApiClient")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<SeatsIo>()
    .BindConfiguration("SeatsIo")
    .ValidateDataAnnotations()
    .ValidateOnStart();

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
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.UseAuthentication();
app.UseAuthorization();

// Map health check endpoint for container health monitoring
app.MapHealthChecks("/healthz");

app.Run();
