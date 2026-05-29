using System.Security.Claims;
using System.Text.Json;
using Odasoft.XBOL.Auth;

namespace Odasoft.XBOL.AdminPortal.Services;

public static class AdminClaimsBuilder
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<AdminProfile?> FetchProfileAsync(
        HttpClient client,
        string sessionCookie,
        string cookieName,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "auth/me");
        request.Headers.Add("Cookie", $"{cookieName}={sessionCookie}");

        using var response = await client.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<AdminProfile>(json, JsonOptions);
    }

    public static List<Claim> BuildClaims(AdminProfile profile)
    {
        var claims = new List<Claim>
        {
            new(AdminClaimTypes.LocalUserId, profile.Id.ToString())
        };

        foreach (var role in profile.Roles)
        {
            if (!string.IsNullOrWhiteSpace(role.Code))
            {
                claims.Add(new Claim(AdminClaimTypes.Role, role.Code));
            }
        }

        foreach (var permission in profile.Permissions)
        {
            if (!string.IsNullOrWhiteSpace(permission))
            {
                claims.Add(new Claim(AdminClaimTypes.Permission, permission));
            }
        }

        return claims;
    }
}

public sealed class AdminProfile
{
    public Guid Id { get; set; }
    public List<RoleSummary> Roles { get; set; } = [];
    public List<string> Permissions { get; set; } = [];
}

public sealed class RoleSummary
{
    public string Code { get; set; } = "";
}
