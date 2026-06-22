using Odasoft.XBOL.AdminPortal.ViewModels.Reports;
using System.Text.Json;

namespace Odasoft.XBOL.AdminPortal.Services.Contracts;

public interface IExecutiveReportsApiClient
{
    Task<IReadOnlyList<ReportTypeOption>> GetReportTypeOptionsAsync(CancellationToken token = default);
    Task<IReadOnlyList<SelectOption>> GetCashiersAsync(long eventId, CancellationToken token = default);
    Task<IReadOnlyList<SelectOption>> GetClientsAsync(CancellationToken token = default);
    Task<ReportListResponse<Dictionary<string, JsonElement>>> GetReportListAsync(ReportFilter filter, CancellationToken token = default);
    Task<byte[]> DownloadReportAsync(ReportFilter filter, CancellationToken token = default);
    Task<byte[]> DownloadPdfReportAsync(ReportFilter filter, CancellationToken token = default);
}
