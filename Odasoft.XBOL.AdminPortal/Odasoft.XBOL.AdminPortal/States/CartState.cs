namespace Odasoft.XBOL.AdminPortal.States
{
    public class CartState
    {
        private List<SeatInfo> selectedSeats = [];
        private decimal totalPrice;
        private long? currentEventId;

        public event Action? OnChange;

        public long? CurrentEventId => currentEventId;

        public void SetEvent(long eventId)
        {
            if (currentEventId != eventId)
            {
                ClearCart();
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
