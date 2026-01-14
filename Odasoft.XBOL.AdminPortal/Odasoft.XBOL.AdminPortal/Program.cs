using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using MudBlazor.Translations;
using Odasoft.XBOL.AdminPortal;
using Odasoft.XBOL.AdminPortal.Components;
using Odasoft.XBOL.AdminPortal.Configs;
using Odasoft.XBOL.AdminPortal.Services;
using Odasoft.XBOL.AdminPortal.Services.Contracts;
using Odasoft.XBOL.AdminPortal.States;

var builder = WebApplication.CreateBuilder(args);

#region AppSettings
Authentication authenticationConfig = builder.Configuration.GetSection("Authentication").Get<Authentication>()!;
#endregion

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.TopCenter;
    config.SnackbarConfiguration.SnackbarVariant = MudBlazor.Variant.Text;
    config.SnackbarConfiguration.MaxDisplayedSnackbars = 5;
});

builder.Services.AddHealthChecks();

#region Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    string[] supportedCultures = ["es"];
    options.SetDefaultCulture("es");
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});
#endregion

#region Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/login";
});

builder.Services.AddAuthorization();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<AuthStateProvider>();
#endregion

#region Services
builder.Services.AddScoped<IAuthService, AuthService>();
#endregion

#region Configs DI
builder.Services.AddSingleton(authenticationConfig);
#endregion

builder.Services.AddHttpContextAccessor();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options =>
    {
        options.DetailedErrors = true;
    });
builder.Services.AddMudTranslations();

// Services
builder.Services.AddScoped<IEventService, ApiEventService>();

builder.Services.AddScoped<GeneralService>();
builder.Services.AddSingleton<CartState>();

builder.Services.AddHttpClient<IAdminApiClient, AdminApiClient>(
    (provider, client) =>
    {
        client.BaseAddress = new Uri(builder.Configuration.GetValue(
            "AdminApiClientBaseAddress", "https://localhost:7241/"));
    });

// Services (External)
builder.Services.AddOptions<SeatsIo>()
    .BindConfiguration("SeatsIo")
    .ValidateDataAnnotations()
    .ValidateOnStart();

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

app.UseAuthentication();
app.UseAuthorization();

// Map health check endpoint for container health monitoring
app.MapHealthChecks("/healthz");

app.Run();
