namespace Odasoft.XBOL.Auth;

[Flags]
public enum EventReadScope
{
    None = 0,
    Current = 1,
    Future = 2,
    Past = 4,
    All = Current | Future | Past
}
