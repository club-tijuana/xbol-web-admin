using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using Odasoft.XBOL.Common.Options;
using System.Net;
using AppForwardedHeadersOptions = Odasoft.XBOL.Common.Options.ForwardedHeadersOptions;
using AspNetForwardedHeadersOptions = Microsoft.AspNetCore.Builder.ForwardedHeadersOptions;
using IPNetwork = Microsoft.AspNetCore.HttpOverrides.IPNetwork;

namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class HostingConfiguration
{
    /// <summary>
    /// Wires up reverse-proxy path base handling from Hosting:PathBase.
    /// UsePathBase handles requests that still contain the prefix (direct dev
    /// hits / proxy-preserves); a follow-up middleware forces Request.PathBase
    /// to the configured value when the reverse proxy has already stripped the
    /// prefix, so NavigationManager.BaseUri and LinkGenerator still produce
    /// URLs that round-trip through the proxy. No-op when PathBase is unset.
    /// </summary>
    public static IApplicationBuilder UseConfiguredPathBase(this IApplicationBuilder app)
    {
        var pathBase = app.ApplicationServices
            .GetRequiredService<IOptions<HostingOptions>>().Value.PathBase;

        if (string.IsNullOrEmpty(pathBase))
        {
            return app;
        }

        app.UsePathBase(pathBase);
        app.Use((context, next) =>
        {
            if (!context.Request.PathBase.HasValue)
            {
                context.Request.PathBase = pathBase;
            }
            return next(context);
        });

        return app;
    }

    /// <summary>
    /// Configures the framework's ForwardedHeadersMiddleware options from
    /// Hosting:ForwardedHeaders. No-op when the section is missing or disabled.
    /// </summary>
    public static IServiceCollection ConfigureHosting(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AspNetForwardedHeadersOptions>(opts =>
        {
            var cfg = configuration
                .GetSection("Hosting:ForwardedHeaders")
                .Get<AppForwardedHeadersOptions>();

            if (cfg is null || !cfg.Enabled)
            {
                return;
            }

            opts.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto |
                ForwardedHeaders.XForwardedHost;

            // Clear defaults (loopback only) — replace with explicit config if provided,
            // otherwise trust any upstream (safe when the container is only reachable via the proxy).
            opts.KnownProxies.Clear();
            opts.KnownNetworks.Clear();

            foreach (var ip in cfg.KnownProxies ?? [])
            {
                opts.KnownProxies.Add(IPAddress.Parse(ip));
            }

            foreach (var cidr in cfg.KnownNetworks ?? [])
            {
                var parts = cidr.Split('/', 2);
                opts.KnownNetworks.Add(new IPNetwork(
                    IPAddress.Parse(parts[0]),
                    int.Parse(parts[1])));
            }
        });

        return services;
    }
}
