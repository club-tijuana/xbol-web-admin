namespace Odasoft.XBOL.AdminPortal.ViewModels;

public record EventFilterParameters(
    List<string> Venues,
    List<string> Types,
    DateTime? DateFrom,
    DateTime? DateTo
)
{
    public static EventFilterParameters Empty => new([], [], null, null);

    public bool HasAnyFilter() =>
        Venues.Count > 0 || Types.Count > 0 || DateFrom.HasValue || DateTo.HasValue;
}
