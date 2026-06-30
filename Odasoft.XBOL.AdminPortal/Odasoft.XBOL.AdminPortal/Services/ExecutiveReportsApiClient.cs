using Odasoft.XBOL.AdminPortal.Services.Contracts;
using Odasoft.XBOL.AdminPortal.ViewModels.Reports;
using Odasoft.XBOL.Business;
using System.Text.Json;
using ReportEventOption = Odasoft.XBOL.AdminPortal.ViewModels.Reports.ReportEventOption;
using ReportFilter = Odasoft.XBOL.AdminPortal.ViewModels.Reports.ReportFilter;
using SelectOption = Odasoft.XBOL.AdminPortal.ViewModels.Reports.SelectOption;

namespace Odasoft.XBOL.AdminPortal.Services;

public class ExecutiveReportsApiClient(IAdminClient adminClient, HttpClient httpClient) : IExecutiveReportsApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<IReadOnlyList<ReportTypeOption>> GetReportTypeOptionsAsync(CancellationToken token = default)
    {
        var result = await adminClient.GetReportTypeOptionsAsync(token);
        return result.Select(o => new ReportTypeOption { Value = o.Value!, Label = o.Label! }).ToList();
    }

    public async Task<IReadOnlyList<SelectOption>> GetCashiersAsync(long? eventId, long? bundleId, CancellationToken token = default)
    {
        var query = eventId.HasValue
            ? $"eventId={eventId.Value}"
            : $"bundleId={bundleId!.Value}";
        var response = await httpClient.GetAsync($"api/executive-reports/cashiers?{query}", token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IReadOnlyList<SelectOption>>(JsonOptions, token) ?? [];
    }

    public async Task<IReadOnlyList<SelectOption>> GetClientsAsync(CancellationToken token = default)
    {
        var response = await httpClient.GetAsync("api/executive-reports/clients", token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IReadOnlyList<SelectOption>>(JsonOptions, token) ?? [];
    }

    public async Task<IReadOnlyList<SelectOption>> GetBundlesAsync(CancellationToken token = default)
    {
        var response = await httpClient.GetAsync("api/executive-reports/bundles", token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IReadOnlyList<SelectOption>>(JsonOptions, token) ?? [];
    }

    public async Task<IReadOnlyList<ViewModels.Reports.ReportEventOption>> GetEventsForReportAsync(CancellationToken token = default)
    {
        var response = await httpClient.GetAsync("api/executive-reports/events", token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IReadOnlyList<ViewModels.Reports.ReportEventOption>>(JsonOptions, token) ?? [];
    }

    public async Task<ReportListResponse<Dictionary<string, JsonElement>>> GetReportListAsync(ReportFilter filter, CancellationToken token = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/executive-reports/list", filter, token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ReportListResponse<Dictionary<string, JsonElement>>>(JsonOptions, token) ?? new();
    }

    public async Task<byte[]> DownloadReportAsync(ReportFilter filter, CancellationToken token = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/executive-reports/report", filter, token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync(token);
    }

    public async Task<byte[]> DownloadPdfReportAsync(ReportFilter filter, CancellationToken token = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/executive-reports/report-pdf", filter, token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync(token);
    }
}
