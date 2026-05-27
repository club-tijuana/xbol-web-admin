using Odasoft.XBOL.AdminPortal.Services.Contracts;
using Odasoft.XBOL.AdminPortal.ViewModels.Reports;
using System.Text.Json;

namespace Odasoft.XBOL.AdminPortal.Services;

public class ExecutiveReportsApiClient(HttpClient httpClient) : IExecutiveReportsApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<IReadOnlyList<ReportTypeOption>> GetReportTypeOptionsAsync(CancellationToken token = default)
    {
        var response = await httpClient.GetAsync("api/executive-reports/report-types", token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<ReportTypeOption>>(JsonOptions, token) ?? [];
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
}
