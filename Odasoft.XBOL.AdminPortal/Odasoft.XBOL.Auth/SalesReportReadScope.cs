namespace Odasoft.XBOL.Auth;

[Flags]
public enum SalesReportReadScope
{
    None = 0,
    Self = 1,
    Cashiers = 2,
    Web = 4,
    All = Self | Cashiers | Web
}
