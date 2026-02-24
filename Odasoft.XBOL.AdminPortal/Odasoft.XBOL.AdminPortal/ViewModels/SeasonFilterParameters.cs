namespace Odasoft.XBOL.AdminPortal.ViewModels;

public record SeasonFilterParameters(
    DateTimeOffset? EndDateMin = null,  // Seasons ending on or after this (current + future)
    DateTimeOffset? EndDateMax = null,  // Seasons ending before this (past)
    SeasonStatus? Status = null
)
{
    public static SeasonFilterParameters Empty => new();

    public bool HasAnyFilter() =>
        EndDateMin.HasValue || EndDateMax.HasValue || Status.HasValue;
}
