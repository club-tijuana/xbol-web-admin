namespace Odasoft.XBOL.AdminPortal.Services;

public sealed record FirebaseAuthUser(
    string Uid,
    string? Email,
    string? DisplayName,
    string IdToken);
