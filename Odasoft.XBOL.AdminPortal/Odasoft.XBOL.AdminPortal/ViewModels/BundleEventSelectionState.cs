namespace Odasoft.XBOL.AdminPortal.ViewModels;

public sealed class BundleEventSelectionState
{
    private readonly Dictionary<long, EventViewModel> _selectedByScheduleId = [];
    private readonly HashSet<long> _persistedScheduleIds = [];

    public IReadOnlyCollection<long> PersistedEventScheduleIds => _persistedScheduleIds;
    public int Count => _selectedByScheduleId.Count;

    public IEnumerable<EventViewModel> SelectedEvents =>
        _selectedByScheduleId.Values.OrderBy(item => item.StartDateTime);

    public void LoadPersisted(IEnumerable<EventViewModel> events)
    {
        _selectedByScheduleId.Clear();
        _persistedScheduleIds.Clear();

        foreach (var item in events)
        {
            if (item.EventScheduleId is long scheduleId)
            {
                _selectedByScheduleId[scheduleId] = item;
                _persistedScheduleIds.Add(scheduleId);
            }
        }
    }

    public void ClearSelection()
    {
        _selectedByScheduleId.Clear();
    }

    public void Add(EventViewModel item)
    {
        if (item.EventScheduleId is long scheduleId)
        {
            _selectedByScheduleId[scheduleId] = item;
        }
    }

    public void Remove(EventViewModel item)
    {
        if (item.EventScheduleId is long scheduleId)
        {
            _selectedByScheduleId.Remove(scheduleId);
        }
    }

    public bool IsPersisted(long scheduleId) => _persistedScheduleIds.Contains(scheduleId);

    public IEnumerable<EventViewModel> AvailableEvents(IEnumerable<EventViewModel> candidates)
    {
        return candidates.Where(item =>
            item.EventScheduleId is long scheduleId &&
            !_selectedByScheduleId.ContainsKey(scheduleId));
    }

    public BundleEventScheduleDelta GetDelta(bool skipRemovals)
    {
        var selectedScheduleIds = SelectedEvents
            .Select(item => item.EventScheduleId!.Value)
            .ToList();
        var selectedScheduleIdSet = selectedScheduleIds.ToHashSet();

        var removedScheduleIds = skipRemovals
            ? []
            : _persistedScheduleIds
                .Except(selectedScheduleIdSet)
                .ToList();

        var addedScheduleIds = selectedScheduleIds
            .Where(scheduleId => !_persistedScheduleIds.Contains(scheduleId))
            .ToList();

        return new BundleEventScheduleDelta(addedScheduleIds, removedScheduleIds);
    }

    public void AcceptCurrentSelectionAsPersisted()
    {
        _persistedScheduleIds.Clear();

        foreach (var scheduleId in _selectedByScheduleId.Keys)
        {
            _persistedScheduleIds.Add(scheduleId);
        }
    }
}

public sealed record BundleEventScheduleDelta(
    IReadOnlyList<long> AddedEventScheduleIds,
    IReadOnlyList<long> RemovedEventScheduleIds);
