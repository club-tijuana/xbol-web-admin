namespace Odasoft.XBOL.AdminPortal.ViewModels;

public enum SeasonStatus
{
    Draft,
    Published,
    Closed
}

public record SeasonViewModel(
    long Id,
    string Code,
    string Name,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    SeasonStatus Status
);
