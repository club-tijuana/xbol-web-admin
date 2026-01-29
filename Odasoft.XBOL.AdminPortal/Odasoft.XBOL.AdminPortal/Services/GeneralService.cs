using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.Services
{
    public class GeneralService(IAdminClient adminClient)
    {
        public async Task<BookingResult> BookTickets(string eventId, List<string> seats, string holdToken)
        {
            var request = new BookingRequest
            {
                Seats = seats,
                HoldToken = holdToken,
                EventId = eventId
            };

            return await adminClient.BookSeatsAsync(request);
        }
    }
}