using Odasoft.XBOL.AdminPortal.ViewModels;
using Odasoft.XBOL.Business;
using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class BundleEventSelectionStateTests
{
    [Fact]
    public void Add_new_schedule_produces_add_delta()
    {
        var state = new BundleEventSelectionState();

        state.LoadPersisted([Event(10, "Persisted", daysFromNow: 1)]);
        state.Add(Event(20, "New", daysFromNow: 2));

        var delta = state.GetDelta(skipRemovals: false);

        Assert.Equal([20L], delta.AddedEventScheduleIds);
        Assert.Empty(delta.RemovedEventScheduleIds);
    }

    [Fact]
    public void Removing_persisted_schedule_produces_remove_delta()
    {
        var state = new BundleEventSelectionState();

        state.LoadPersisted([Event(10, "Persisted", daysFromNow: 1)]);
        state.Remove(Event(10, "Persisted", daysFromNow: 1));

        var delta = state.GetDelta(skipRemovals: false);

        Assert.Empty(delta.AddedEventScheduleIds);
        Assert.Equal([10L], delta.RemovedEventScheduleIds);
    }

    [Fact]
    public void Persisted_selected_schedule_remains_selected_when_not_in_candidate_list()
    {
        var state = new BundleEventSelectionState();

        state.LoadPersisted([Event(10, "Persisted", daysFromNow: 1)]);

        Assert.Equal([10L], state.SelectedEvents.Select(item => item.EventScheduleId!.Value));
        Assert.Equal([20L], state.AvailableEvents([Event(20, "Candidate", daysFromNow: 2)]).Select(item => item.EventScheduleId!.Value));
    }

    [Fact]
    public void Accepting_delta_marks_selection_as_persisted()
    {
        var state = new BundleEventSelectionState();

        state.LoadPersisted([Event(10, "Persisted", daysFromNow: 1)]);
        state.Add(Event(20, "New", daysFromNow: 2));
        state.AcceptCurrentSelectionAsPersisted();

        var delta = state.GetDelta(skipRemovals: false);

        Assert.Empty(delta.AddedEventScheduleIds);
        Assert.Empty(delta.RemovedEventScheduleIds);
        Assert.Equal([10L, 20L], state.PersistedEventScheduleIds.Order());
    }

    private static EventViewModel Event(long scheduleId, string name, int daysFromNow)
    {
        return new EventViewModel(
            Id: scheduleId + 100,
            StartDateTime: DateTime.Today.AddDays(daysFromNow),
            EndDateTime: null,
            EventName: name,
            Categories: "",
            Venue: "Venue",
            Available: 10,
            Total: 20,
            Status: null,
            ExternalEventKey: null,
            EventScheduleId: scheduleId,
            VenueMapId: 5,
            ScheduleStatus: ScheduleStatus.Draft);
    }
}
