using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.States
{
    // TODO: Find a better way to implement this
    public class PersistentDialogState
    {
        public BookingResult? BookingResult { get; private set; }

        public void SetBookingResult(BookingResult result)
        {
            BookingResult = result;
            NotifyStateChanged();
        }

        public void ClearBookingResult()
        {
            BookingResult = null;
            NotifyStateChanged();
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
