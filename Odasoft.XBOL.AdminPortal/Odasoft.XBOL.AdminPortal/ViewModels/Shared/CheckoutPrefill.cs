using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.ViewModels.Shared;

public record CheckoutPrefill(
    long? ClientId,
    long? PhoneRegionCodeId,
    string? PhoneNumber,
    string? Email,
    string? FullName,
    string? City,
    string? Neighborhood,
    ClientGender? Gender,
    DateTime? Birthday);
