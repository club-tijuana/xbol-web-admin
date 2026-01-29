using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.ViewModels;

public class SeasonPassFilterParameters
{
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public bool ShowRenewalMode { get; set; }

    public List<SeasonPassItemStatus?> RenewalTypes = new();

    public static SeasonPassFilterParameters Empty => new();

    public bool HasAnyFilter() =>
        DateFrom.HasValue ||
        DateTo.HasValue;
}
