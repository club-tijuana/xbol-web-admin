namespace Odasoft.XBOL.AdminPortal.ViewModels.Reports;

public class ReportSummaryItem
{
    public string Key { get; set; } = null!;
    public string Label { get; set; } = null!;
    public object? Value { get; set; }
    public string? FormattedValue { get; set; }
    public string? Format { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public string? Subtitle { get; set; }
}
