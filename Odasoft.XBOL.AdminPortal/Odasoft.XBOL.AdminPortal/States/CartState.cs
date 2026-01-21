namespace Odasoft.XBOL.AdminPortal.States
{
    public class CartState
    {
        public event Action? OnChange;

        public long? CurrentEventId => currentEventId;
        public string? HoldToken { get; private set; }
        public bool IsHoldCallsInProgress { get; private set; }
        public DateTime? HoldExpirationTime { get; private set; }

        private List<SeatInfo> selectedSeats = [];
        private decimal totalPrice;
        private long? currentEventId;

        public void SetEvent(long eventId)
        {
            if (currentEventId != eventId)
            {
                ClearCart();
                ClearToken();
                currentEventId = eventId;
            }
        }

        public void ClearCart()
        {
            selectedSeats.Clear();
            totalPrice = 0;
            NotifyStateChanged();
        }

        public List<SeatInfo> SelectedSeats
        {
            get => selectedSeats;
            set
            {
                selectedSeats = value;
                TotalPrice = selectedSeats.Sum(seat => seat.Price);
                NotifyStateChanged();
            }
        }

        public decimal TotalPrice
        {
            get => totalPrice;
            private set => totalPrice = value;
        }

        public void AddSelectedSeat(SeatInfo item)
        {
            if (!SelectedSeats.Any(x => x.SeatId == item.SeatId))
            {
                SelectedSeats.Add(item);
                TotalPrice = selectedSeats.Sum(seat => seat.Price);
                NotifyStateChanged();
            }
        }

        public void RemoveSelectedSeat(string seatId)
        {
            if (SelectedSeats.Any(x => x.SeatId == seatId))
            {
                SelectedSeats.RemoveAll(seat => seat.SeatId == seatId);
                TotalPrice = selectedSeats.Sum(seat => seat.Price);
                NotifyStateChanged();
            }
        }

        public void SetHoldToken(string token, int expiresInSeconds)
        {
            if (HoldToken != token)
            {
                HoldToken = token;
                // Reset timer on new token or refresh
                HoldExpirationTime = DateTime.UtcNow.AddSeconds(expiresInSeconds);
                NotifyStateChanged();
            }
        }

        public void ClearToken()
        {
            HoldToken = null;
            HoldExpirationTime = null;
            NotifyStateChanged();
        }

        public void SetHoldBusyState(bool isBusy)
        {
            if (IsHoldCallsInProgress != isBusy)
            {
                IsHoldCallsInProgress = isBusy;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

    // TODO: Move to an appropiate place in project
    public class SeatInfo
    {
        public required string SeatId { get; set; }
        public decimal Price { get; set; }
        public string? Category { get; set; }
    }
}
