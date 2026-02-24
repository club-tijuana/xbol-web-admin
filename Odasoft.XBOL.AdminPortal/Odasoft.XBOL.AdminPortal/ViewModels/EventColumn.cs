namespace Odasoft.XBOL.AdminPortal.ViewModels;

[Flags]
public enum EventColumn
{
    None         = 0,
    DateTime     = 1 << 0,
    Category     = 1 << 1,
    Availability = 1 << 2,

    All = DateTime | Category | Availability
}
