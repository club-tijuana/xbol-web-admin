using Microsoft.AspNetCore.Http;
using Odasoft.XBOL.AdminPortal.Extensions;
using Odasoft.XBOL.Common.Options;
using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class ApiDocsProxyTests
{
    [Fact]
    public void ResolveTarget_uses_known_service_base_addresses()
    {
        var options = new ApiDocsProxyOptions
        {
            TicketingApiBaseAddress = "http://ticketing/",
            ClientApiBaseAddress = "http://client/"
        };

        var admin = ApiDocsProxy.ResolveTarget("admin-api", options, "http://admin/");
        var ticketing = ApiDocsProxy.ResolveTarget("ticketing-api", options, "http://admin/");
        var client = ApiDocsProxy.ResolveTarget("client-api", options, "http://admin/");

        Assert.Equal("http://admin/", admin.BaseAddress);
        Assert.Equal("admin-api", admin.DocumentName);
        Assert.Equal("http://ticketing/", ticketing.BaseAddress);
        Assert.Equal("ticketing-api", ticketing.DocumentName);
        Assert.Equal("http://client/", client.BaseAddress);
        Assert.Equal("client-api", client.DocumentName);
    }

    [Theory]
    [InlineData(null, "/swagger/index.html")]
    [InlineData("", "/swagger/index.html")]
    [InlineData("index.html", "/swagger/index.html")]
    [InlineData("v1/admin-api.json", "/swagger/v1/admin-api.json")]
    [InlineData("swagger-ui.css", "/swagger/swagger-ui.css")]
    public void BuildUpstreamPath_maps_public_docs_path_to_upstream_swagger_path(
        string? publicPath,
        string expectedPath)
    {
        Assert.Equal(expectedPath, ApiDocsProxy.BuildUpstreamPath(publicPath));
    }

    [Fact]
    public void ValidatePath_rejects_dot_segments()
    {
        Assert.Throws<BadHttpRequestException>(() => ApiDocsProxy.ValidatePath("../swagger.json"));
        Assert.Throws<BadHttpRequestException>(() => ApiDocsProxy.ValidatePath("%2e%2e/swagger.json"));
    }

    [Fact]
    public void RewriteResponseBodyForDocs_keeps_references_under_admin_docs_path()
    {
        const string body = """
            <script src="/swagger/swagger-ui-bundle.js"></script>
            <script>const url = "/swagger/v1/admin-api.json";</script>
            """;

        var rewritten = ApiDocsProxy.RewriteResponseBodyForDocs(
            body,
            new PathString("/admin"),
            "admin-api");

        Assert.Contains("/admin/docs/admin-api/swagger-ui-bundle.js", rewritten, StringComparison.Ordinal);
        Assert.Contains("/admin/docs/admin-api/v1/admin-api.json", rewritten, StringComparison.Ordinal);
        Assert.DoesNotContain("\"/swagger/", rewritten, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("/swagger", "/admin/docs/admin-api")]
    [InlineData("/swagger/index.html", "/admin/docs/admin-api/index.html")]
    [InlineData("/swagger/v1/admin-api.json", "/admin/docs/admin-api/v1/admin-api.json")]
    public void RewriteLocationHeader_keeps_swagger_redirects_under_admin_docs_path(
        string location,
        string expectedLocation)
    {
        var rewritten = ApiDocsProxy.RewriteLocationHeader(
            location,
            new PathString("/admin"),
            "admin-api");

        Assert.Equal(expectedLocation, rewritten);
    }

    [Fact]
    public void Api_docs_route_requires_docs_permission()
    {
        var source = File.ReadAllText(
            Path.Combine(
                AppContext.BaseDirectory,
                "..",
                "..",
                "..",
                "..",
                "Odasoft.XBOL.AdminPortal",
                "Extensions",
                "ApiDocsProxyEndpointConfiguration.cs"));

        Assert.Contains("MapGroup(\"/docs/{service}\")", source, StringComparison.Ordinal);
        Assert.Contains(
            ".RequireAuthorization(PermissionPolicy.Build(AdminPermissions.Docs.Read))",
            source,
            StringComparison.Ordinal);
    }
}
