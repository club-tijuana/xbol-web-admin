using System.Text.Json;
using EventCatalogItemType = Odasoft.XBOL.Business.EventCatalogItemType;
using ReportFilter = Odasoft.XBOL.AdminPortal.ViewModels.Reports.ReportFilter;
using ReportListResponse = Odasoft.XBOL.AdminPortal.ViewModels.Reports.ReportListResponse<System.Collections.Generic.Dictionary<string, System.Text.Json.JsonElement>>;
using ReportTypeOption = Odasoft.XBOL.AdminPortal.ViewModels.Reports.ReportTypeOption;
using SelectOption = Odasoft.XBOL.AdminPortal.ViewModels.Reports.SelectOption;

namespace Odasoft.XBOL.AdminPortal.Services.Contracts;

public interface IExecutiveReportsApiClient
{
    Task<IReadOnlyList<ReportTypeOption>> GetReportTypeOptionsAsync(CancellationToken token = default);
    Task<IReadOnlyList<SelectOption>> GetCashiersAsync(long eventId, EventCatalogItemType catalogItemType = EventCatalogItemType.Event, CancellationToken token = default);
    Task<IReadOnlyList<SelectOption>> GetClientsAsync(CancellationToken token = default);
    Task<ReportListResponse> GetReportListAsync(ReportFilter filter, CancellationToken token = default);
    Task<byte[]> DownloadReportAsync(ReportFilter filter, CancellationToken token = default);
    Task<byte[]> DownloadPdfReportAsync(ReportFilter filter, CancellationToken token = default);
}
