using System.Security.Claims;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.Options;
using Odasoft.XBOL.Common.Options;

namespace Odasoft.XBOL.AdminPortal.Services;

public sealed class AdminAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
{
    private static readonly TimeSpan PermissionRefreshInterval = TimeSpan.FromMinutes(5);

    private readonly FirebaseAuth _firebaseAuth;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AdminSessionCookieOptions _sessionOptions;
    private readonly ILogger<AdminAuthenticationStateProvider> _logger;
    private readonly SemaphoreSlim _refreshLock = new(1, 1);
    private DateTimeOffset _lastPermissionRefresh;
    private string? _cachedSessionCookie;

    public AdminAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        FirebaseAuth firebaseAuth,
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor,
        IOptions<AdminSessionCookieOptions> sessionOptions)
        : base(loggerFactory)
    {
        _firebaseAuth = firebaseAuth;
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
        _sessionOptions = sessionOptions.Value;
        _logger = loggerFactory.CreateLogger<AdminAuthenticationStateProvider>();
    }

    // Revalidation interval for cross-tab logout detection.
    // See: https://auth0.com/blog/blazor-server-and-the-logout-problem/
    protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(10);

    protected override async Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState authenticationState,
        CancellationToken cancellationToken)
    {
        var user = authenticationState.User;

        if (user.Identity?.IsAuthenticated != true)
        {
            return false;
        }

        CaptureSessionCookie();

        if (_cachedSessionCookie is null)
        {
            return false;
        }

        try
        {
            await _firebaseAuth.VerifySessionCookieAsync(
                _cachedSessionCookie,
                _sessionOptions.CheckRevoked,
                cancellationToken);
        }
        catch (FirebaseAuthException)
        {
            _logger.LogInformation("Session cookie no longer valid — marking authentication state as invalid");
            _cachedSessionCookie = null;
            return false;
        }

        if (DateTimeOffset.UtcNow - _lastPermissionRefresh > PermissionRefreshInterval)
        {
            _ = Task.Run(() => RefreshClaimsAsync(user, cancellationToken), cancellationToken);
        }

        return true;
    }

    public async Task ForceRefreshAsync(CancellationToken cancellationToken = default)
    {
        var currentState = await GetAuthenticationStateAsync();
        await RefreshClaimsAsync(currentState.User, cancellationToken);
    }

    private async Task RefreshClaimsAsync(ClaimsPrincipal currentPrincipal, CancellationToken cancellationToken)
    {
        await _refreshLock.WaitAsync(cancellationToken);
        try
        {
            CaptureSessionCookie();

            if (_cachedSessionCookie is null)
            {
                _logger.LogWarning("No session cookie available for claims refresh");
                return;
            }

            var profile = await AdminClaimsBuilder.FetchProfileAsync(
                _httpClientFactory.CreateClient("AdminApiSession"),
                _cachedSessionCookie,
                _sessionOptions.CookieName,
                cancellationToken);

            if (profile is null)
            {
                _logger.LogWarning("Failed to refresh claims from auth/me");
                return;
            }

            var baseIdentity = (ClaimsIdentity)currentPrincipal.Identity!;
            var enrichedIdentity = new ClaimsIdentity(
                baseIdentity.Claims,
                baseIdentity.AuthenticationType,
                baseIdentity.NameClaimType,
                baseIdentity.RoleClaimType);

            RemoveExistingClaims(enrichedIdentity, Auth.AdminClaimTypes.LocalUserId);
            RemoveExistingClaims(enrichedIdentity, Auth.AdminClaimTypes.Role);
            RemoveExistingClaims(enrichedIdentity, Auth.AdminClaimTypes.Permission);

            enrichedIdentity.AddClaims(AdminClaimsBuilder.BuildClaims(profile));

            var enrichedState = new AuthenticationState(new ClaimsPrincipal(enrichedIdentity));
            _lastPermissionRefresh = DateTimeOffset.UtcNow;

            NotifyAuthenticationStateChanged(Task.FromResult(enrichedState));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing claims from auth/me");
        }
        finally
        {
            _refreshLock.Release();
        }
    }

    private void CaptureSessionCookie()
    {
        if (_cachedSessionCookie is not null)
        {
            return;
        }

        var context = _httpContextAccessor.HttpContext;
        if (context?.Request.Cookies.TryGetValue(_sessionOptions.CookieName, out var cookie) == true
            && !string.IsNullOrWhiteSpace(cookie))
        {
            _cachedSessionCookie = cookie;
        }
    }

    private static void RemoveExistingClaims(ClaimsIdentity identity, string claimType)
    {
        var claims = identity.FindAll(claimType).ToList();
        foreach (var claim in claims)
        {
            identity.RemoveClaim(claim);
        }
    }
}
