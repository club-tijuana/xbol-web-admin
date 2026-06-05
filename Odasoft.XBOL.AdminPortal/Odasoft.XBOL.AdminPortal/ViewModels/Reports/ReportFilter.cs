namespace Odasoft.XBOL.AdminPortal.ViewModels.Reports;

public class ReportFilter
{
    public string ReportType { get; set; } = string.Empty;
    public long EventId { get; set; }
    public Guid? CashierId { get; set; }
    public long? ClientId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public string? SortBy { get; set; }
    public string SortDirection { get; set; } = "Ascending";
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
}
