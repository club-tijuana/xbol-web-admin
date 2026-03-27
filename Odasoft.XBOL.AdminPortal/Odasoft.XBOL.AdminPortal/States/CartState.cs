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

        public List<string> SelectedSeatsKeys => FinalSeatsSelection
            .Select(x => x.SeatId)
            .ToList();

        public List<SeatInfo> FinalSeatsSelection => SelectedSeats
            .Where(x => x.IsSelected == true && x.IsSold != true)
            .ToList();

        public bool? AllSeatsSelected { get; private set; }

        public decimal TotalPrice
        {
            get => totalPrice;
            private set => totalPrice = value;
        }

        public List<SeatInfo> SelectedSeats
        {
            get => selectedSeats;
            set
            {
                selectedSeats = value;
                RecalculateState();
            }
        }

        public void SetEvent(long eventId)
        {
            if (currentEventId != eventId)
            {
                ClearCart();
                ClearTokenAsync();
                currentEventId = eventId;
            }
        }

        public void LoadOrder(long newEventId, List<SeatInfo> seats)
        {
            SetEvent(newEventId);

            foreach (var seat in seats)
            {
                seat.IsSelected = seat.IsSold != true;
            }

            SelectedSeats = seats;
        }

        public void ClearCart()
        {
            SelectedSeats.Clear();
            RecalculateState();
            NotifyStateChanged();
        }

        public void AddSelectedSeat(SeatInfo item)
        {
            var existingSeat = SelectedSeats.FirstOrDefault(x => x.SeatId == item.SeatId);

            if (existingSeat != null)
            {
                if (existingSeat.IsSold != true)
                {
                    existingSeat.IsSelected = true;
                    existingSeat.Price = item.Price;
                    RecalculateState();
                }
            }
            else
            {
                item.IsSelected = true;
                SelectedSeats.Add(item);
                RecalculateState();
            }
        }

        public void RemoveSelectedSeat(string seatId)
        {
            var seat = SelectedSeats.FirstOrDefault(x => x.SeatId == seatId);
            if (seat is not null && seat.IsSold != true)
            {
                if (seat.SourceOrderItemId is not null)
                {
                    seat.IsSelected = false;
                }
                else
                {
                    selectedSeats.Remove(seat);
                }

                RecalculateState();
            }
        }

        private void RecalculateState()
        {
            TotalPrice = SelectedSeats
                .Where(seat => seat.IsSelected == true && seat.IsSold != true)
                .Sum(seat => seat.Price);

            if (SelectedSeats.All(i => i.IsSelected == true))
            {
                AllSeatsSelected = true;
            }
            else if (SelectedSeats.Any(i => i.IsSelected == true))
            {
                AllSeatsSelected = null;
            }
            else
            {
                AllSeatsSelected = false;
            }

            NotifyStateChanged();
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

        public void SetHoldToken(string token, DateTimeOffset expiresAt)
        {
            if (HoldToken != token)
            {
                HoldToken = token;
                HoldExpirationTime = expiresAt.UtcDateTime;
                NotifyStateChanged();
            }
        }

        public void ClearTokenAsync()
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
        public bool? IsSold { get; set; }
        public bool IsSelected { get; set; }
        public long? SourceOrderItemId { get; set; }
    }
}
