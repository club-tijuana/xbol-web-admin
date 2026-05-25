using Odasoft.XBOL.AdminPortal.States;

namespace Odasoft.XBOL.AdminPortal.ViewModels.Shared;

public record SeasonRenewalPrefill(
    long? RelatedOrderId,
    bool PendingRenewal,
    CheckoutPrefill ClientContact,
    IReadOnlyList<SeatInfo> Seats);
