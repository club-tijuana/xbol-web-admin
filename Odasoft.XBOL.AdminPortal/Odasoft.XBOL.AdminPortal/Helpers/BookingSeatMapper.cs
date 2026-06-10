using Odasoft.XBOL.AdminPortal.States;
using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.Helpers;

public static class BookingSeatMapper
{
    public static List<BookingSeatRequest> ToBookingSeats(IEnumerable<SeatInfo> seats)
    {
        return seats
            .Select(seat => new BookingSeatRequest
            {
                SeatKey = seat.SeatId,
                SeatPrice = seat.Price,
                PriceListItemId = seat.PriceListItemId ?? 0
            })
            .ToList();
    }
}
