using System.Text.Json;
using Odasoft.XBOL.AdminPortal.ViewModels.Reports;

namespace Odasoft.XBOL.AdminPortal.Services.Contracts;

public interface IExecutiveReportsApiClient
{
    Task<IReadOnlyList<ReportTypeOption>> GetReportTypeOptionsAsync(CancellationToken token = default);
    Task<ReportListResponse<Dictionary<string, JsonElement>>> GetReportListAsync(ReportFilter filter, CancellationToken token = default);
    Task<byte[]> DownloadReportAsync(ReportFilter filter, CancellationToken token = default);
}
