using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Odasoft.XBOL.Common.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Odasoft.XBOL.AdminPortal.Services;

public sealed class FirebaseSessionAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "FirebaseSession";

    private const string TenantClaimType = "firebase.tenant";
    private const string FirebaseUidClaimType = "firebase.uid";
    private const string SignInProviderClaimType = "firebase.sign_in_provider";

    private readonly FirebaseAuth _firebaseAuth;
    private readonly GcipAuthOptions _gcipOptions;
    private readonly AdminSessionCookieOptions _sessionOptions;

    public FirebaseSessionAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        FirebaseAuth firebaseAuth,
        IOptions<GcipAuthOptions> gcipOptions,
        IOptions<AdminSessionCookieOptions> sessionOptions)
        : base(options, logger, encoder)
    {
        _firebaseAuth = firebaseAuth;
        _gcipOptions = gcipOptions.Value;
        _sessionOptions = sessionOptions.Value;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Cookies.TryGetValue(_sessionOptions.CookieName, out var sessionCookie)
            || string.IsNullOrWhiteSpace(sessionCookie))
        {
            return AuthenticateResult.NoResult();
        }

        FirebaseToken decoded;
        try
        {
            decoded = await _firebaseAuth.VerifySessionCookieAsync(
                sessionCookie,
                _sessionOptions.CheckRevoked,
                Context.RequestAborted);
        }
        catch (FirebaseAuthException ex)
        {
            return AuthenticateResult.Fail($"Firebase session cookie verification failed: {ex.Message}");
        }

        if (!string.Equals(decoded.TenantId, _gcipOptions.TenantId, StringComparison.Ordinal))
        {
            return AuthenticateResult.Fail($"Session cookie tenant does not match expected tenant '{_gcipOptions.TenantId}'.");
        }

        var principal = new ClaimsPrincipal(new ClaimsIdentity(BuildClaims(decoded), Scheme.Name));
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return AuthenticateResult.Success(ticket);
    }

    private static List<Claim> BuildClaims(FirebaseToken token)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, token.Uid),
            new(FirebaseUidClaimType, token.Uid),
        };

        if (token.TenantId is not null)
        {
            claims.Add(new Claim(TenantClaimType, token.TenantId));
        }

        if (token.Claims.TryGetValue("email", out var email) && email is string emailValue)
        {
            claims.Add(new Claim(ClaimTypes.Email, emailValue));
        }

        if (token.Claims.TryGetValue("name", out var name) && name is string nameValue)
        {
            claims.Add(new Claim(ClaimTypes.Name, nameValue));
        }

        if (token.Claims.TryGetValue("firebase", out var firebase) && firebase is IDictionary<string, object> firebaseClaims)
        {
            if (firebaseClaims.TryGetValue("sign_in_provider", out var provider) && provider is string providerValue)
            {
                claims.Add(new Claim(SignInProviderClaimType, providerValue));
            }
        }

        return claims;
    }
}
