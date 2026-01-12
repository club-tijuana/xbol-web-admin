namespace Odasoft.XBOL.AdminPortal.Services
{
    public class GeneralService(IAdminApiClient apiClient)
    {
        public async Task<BookingResult> BookTickets(string eventId, List<string> seats, string holdToken)
        {
            var request = new BookingRequest
            {
                Seats = seats,
                HoldToken = holdToken,
                EventId = eventId
            };

            return await apiClient.BookingsAsync(request);
        }
    }
}
