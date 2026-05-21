namespace Odasoft.XBOL.AdminPortal.ViewModels.Reports;

public class TransactionsListItem
{
    public DateTime? DateTime { get; set; }
    public string? OrderLocator { get; set; }
    public string? EventName { get; set; }
    public string? SalesChannel { get; set; }
    public string? OperationType { get; set; }
    public string? PaymentMethod { get; set; }
    public decimal? Total { get; set; }
    public string? Status { get; set; }
    public string? ClientName { get; set; }
}
