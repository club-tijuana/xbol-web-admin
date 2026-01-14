namespace Odasoft.XBOL.AdminPortal.ViewModels;

public record EventFilterParameters(
    List<string> Venues,
    List<string> Categories,
    DateTime? DateFrom,
    DateTime? DateTo
)
{
    public static EventFilterParameters Empty => new([], [], null, null);

    public bool HasAnyFilter() =>
        Venues.Count > 0 || Categories.Count > 0 || DateFrom.HasValue || DateTo.HasValue;
}
