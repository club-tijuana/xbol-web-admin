using System.ComponentModel;

namespace Odasoft.XBOL.Common.Options;

public sealed class ApiDocsProxyOptions
{
    public const string SectionName = "ApiDocs";

    [Description("Optional base URL for Admin API docs. Defaults to AdminApiClient:BaseAddress when empty.")]
    public string? AdminApiBaseAddress { get; set; }

    [Description("Base URL for Ticketing API docs.")]
    public string? TicketingApiBaseAddress { get; set; }

    [Description("Base URL for Client API docs.")]
    public string? ClientApiBaseAddress { get; set; }

}
