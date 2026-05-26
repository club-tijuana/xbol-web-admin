namespace Odasoft.XBOL.AdminPortal.ViewModels.Reports;

public class ReportListResponse<TItem>
{
    public List<ReportSummaryItem> Summaries { get; set; } = [];
    public ReportRecordsResult<TItem> Records { get; set; } = null!;
}
