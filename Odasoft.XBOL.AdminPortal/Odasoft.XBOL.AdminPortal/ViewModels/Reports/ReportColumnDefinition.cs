namespace Odasoft.XBOL.AdminPortal.ViewModels.Reports;

public class ReportColumnDefinition
{
    public string Key { get; set; } = null!;
    public string Label { get; set; } = null!;
    public string Type { get; set; } = "text";
    public string? Format { get; set; }
    public string Align { get; set; } = "left";
    public bool Sortable { get; set; } = true;
    public bool Visible { get; set; } = true;
    public bool Filterable { get; set; }
    public bool Searchable { get; set; }
    public string? FilterType { get; set; }
    public Dictionary<string, ReportCellStyle>? StyleMap { get; set; }
}
