using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.States
{
    // TODO: Find a better way to implement this
    public class PersistentDialogState
    {
        public BookingResult? BookingResult { get; private set; }
        public RenewalConfirmation? RenewalConfirmation { get; private set; }

        public void SetBookingResult(BookingResult result, RenewalConfirmation? renewal = null)
        {
            BookingResult = result;
            RenewalConfirmation = renewal;
            NotifyStateChanged();
        }

        public void ClearBookingResult()
        {
            BookingResult = null;
            RenewalConfirmation = null;
            NotifyStateChanged();
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

    public sealed record RenewalConfirmation(
        IReadOnlyList<RenewalSeatLine> SecuredSeats,
        IReadOnlyList<RenewalSeatLine> PendingSeats,
        DateTimeOffset? ReleaseDate);

    public sealed record RenewalSeatLine(string SeatId, string? Category);
}
