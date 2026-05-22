using System.Net.Http.Json;
using System.Text.Json;
using Odasoft.XBOL.AdminPortal.Services.Contracts;
using Odasoft.XBOL.AdminPortal.ViewModels.Reports;
using Odasoft.XBOL.Business;
using System.Text.Json;
using ReportFilter = Odasoft.XBOL.AdminPortal.ViewModels.Reports.ReportFilter;

namespace Odasoft.XBOL.AdminPortal.Services;

public class ExecutiveReportsApiClient(IAdminClient adminClient, HttpClient httpClient) : IExecutiveReportsApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<IReadOnlyList<ReportTypeOption>> GetReportTypeOptionsAsync(CancellationToken token = default)
    {
        var result = await adminClient.GetReportTypeOptionsAsync(token);
        return result.Select(o => new ReportTypeOption { Value = o.Value!, Label = o.Label! }).ToList();
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