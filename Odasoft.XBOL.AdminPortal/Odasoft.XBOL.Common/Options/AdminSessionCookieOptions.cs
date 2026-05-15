using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Odasoft.XBOL.Common.Options;

public sealed class AdminSessionCookieOptions
{
    public static readonly TimeSpan MinLifetime = TimeSpan.FromMinutes(5);
    public static readonly TimeSpan MaxLifetime = TimeSpan.FromDays(14);

    [Required]
    [DefaultValue("__Secure-xbol_admin_session")]
    [Description("Firebase session cookie name issued by the Admin API.")]
    public string CookieName { get; set; } = "__Secure-xbol_admin_session";

    [Description("Cookie domain. Leave empty for host-only cookies.")]
    public string? Domain { get; set; }

    [Required]
    [DefaultValue("/")]
    [Description("Cookie path.")]
    public string Path { get; set; } = "/";

    [DefaultValue("Lax")]
    [Description("Cookie SameSite policy.")]
    public string SameSite { get; set; } = "Lax";

    [DefaultValue(true)]
    [Description("Whether the session cookie is Secure.")]
    public bool Secure { get; set; } = true;

    [DefaultValue("5.00:00:00")]
    [Description("Firebase session cookie lifetime.")]
    public TimeSpan Lifetime { get; set; } = TimeSpan.FromDays(5);

    [DefaultValue("00:05:00")]
    [Description("Maximum age of the Firebase ID token auth_time accepted when creating a session.")]
    public TimeSpan RecentSignInWindow { get; set; } = TimeSpan.FromMinutes(5);

    [DefaultValue(false)]
    [Description("Whether session-cookie verification checks Firebase token revocation.")]
    public bool CheckRevoked { get; set; }
}
