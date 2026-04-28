using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Odasoft.XBOL.Common.Options;

public class HostingOptions
{
    [Description("Public path prefix when hosted behind a reverse proxy (e.g. \"/admin\"). Leave empty when serving at the site root.")]
    [RegularExpression(
        @"^(/[A-Za-z0-9._~\-]+)*$",
        ErrorMessage = "PathBase must be empty or start with '/' and contain URL-safe segments with no trailing slash (e.g. '/admin').")]
    public string? PathBase { get; set; }

    [Description("Forwarded headers handling for reverse-proxy deployments. Disabled by default.")]
    public ForwardedHeadersOptions? ForwardedHeaders { get; set; }
}

public class ForwardedHeadersOptions
{
    [Description("Enable ForwardedHeadersMiddleware to honor X-Forwarded-Proto, X-Forwarded-Host, and X-Forwarded-For from the reverse proxy.")]
    public bool Enabled { get; set; }

    [Description("Known proxy IP addresses allowed to set forwarded headers. Leave empty in container/ingress deployments to clear the defaults and trust any upstream (safe when the container is only reachable via the proxy).")]
    public string[]? KnownProxies { get; set; }

    [Description("Known proxy networks in CIDR notation (e.g. \"10.0.0.0/8\") allowed to set forwarded headers. Leave empty to clear defaults.")]
    public string[]? KnownNetworks { get; set; }
}
