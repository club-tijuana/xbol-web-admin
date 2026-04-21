using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.Services
{
    public class EventScheduleService(IAdminClient _adminClient)
    {
        public async Task<EventScheduleResult> CreateScheduleAsync(CreateEventScheduleRequest request) => await _adminClient.CreateScheduleAsync(request);

        public async Task UpdateScheduleAsync(long id, UpdateEventScheduleRequest request) => await _adminClient.UpdateScheduleAsync(id, request);

        public async Task DeleteSchedule(long id) => await _adminClient.DeleteScheduleAsync(id);
    }
}
