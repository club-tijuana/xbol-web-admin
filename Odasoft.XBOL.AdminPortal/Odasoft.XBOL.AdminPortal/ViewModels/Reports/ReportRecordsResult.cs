namespace Odasoft.XBOL.AdminPortal.ViewModels.Reports;

public class ReportRecordsResult<TItem>
{
    public List<ReportColumnDefinition> Columns { get; set; } = [];
    public List<TItem> Items { get; set; } = [];
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}
