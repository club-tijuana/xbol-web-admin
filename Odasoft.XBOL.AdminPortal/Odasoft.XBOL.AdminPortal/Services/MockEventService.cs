using Bogus;
using MudBlazor;
using Odasoft.XBOL.AdminPortal.ViewModels;

namespace Odasoft.XBOL.AdminPortal.Services;

public class MockEventService : IEventService
{
    private readonly List<EventViewModel> _events;

    public MockEventService()
    {
        _events = GenerateEvents(500);
    }

    private static List<EventViewModel> GenerateEvents(int count)
    {
        var venues = new[]
        {
            "Estadio Caliente",
            "Audiorama El Trompo",
            "Teatro del Estado",
            "Auditorio Municipal",
            "Centro de Convenciones",
        };

        var eventFaker = new Faker<EventViewModel>().CustomInstantiator(f => new EventViewModel(
            Guid.NewGuid(),
            f.Date.Soon(),
            $"EVT-{f.Random.Number(100, 999)}",
            f.Commerce.ProductName(),
            f.PickRandom("Concierto", "Deportivo", "Teatro", "Conferencia", "Comedia"),
            f.PickRandom(venues),
            f.Random.Int(0, 500),
            f.Random.Int(500, 1000)
        ));
        return eventFaker.Generate(count);
    }

    public async Task<GridData<EventViewModel>> GetEventsAsync(
        int page,
        int pageSize,
        string? sortColumn,
        bool sortDescending,
        EventFilterParameters? filters = null
    )
    {
        await Task.Delay(1000);

        var filtered = ApplyFilters(_events, filters);
        var sorted = ApplySorting(filtered, sortColumn, sortDescending);

        return Paginate(sorted, page, pageSize);
    }

    private static IEnumerable<EventViewModel> ApplyFilters(
        IEnumerable<EventViewModel> data,
        EventFilterParameters? filters
    )
    {
        if (filters == null || !filters.HasAnyFilter())
        {
            return data;
        }

        var filtered = data;

        if (filters.Venues.Count > 0)
        {
            filtered = filtered.Where(e => filters.Venues.Contains(e.Venue));
        }

        if (filters.Types.Count > 0)
        {
            filtered = filtered.Where(e => filters.Types.Contains(e.Type));
        }

        if (filters.DateFrom.HasValue)
        {
            filtered = filtered.Where(e => e.DateTime >= filters.DateFrom.Value);
        }

        if (filters.DateTo.HasValue)
        {
            filtered = filtered.Where(e =>
                e.DateTime <= filters.DateTo.Value.AddDays(1).AddTicks(-1)
            );
        }

        return filtered;
    }

    private static IEnumerable<EventViewModel> ApplySorting(
        IEnumerable<EventViewModel> data,
        string? sortColumn,
        bool descending
    )
    {
        if (string.IsNullOrEmpty(sortColumn))
        {
            return data;
        }

        return sortColumn switch
        {
            "DateTime" => descending
                ? data.OrderByDescending(x => x.DateTime)
                : data.OrderBy(x => x.DateTime),
            "EventName" => descending
                ? data.OrderByDescending(x => x.EventName)
                : data.OrderBy(x => x.EventName),
            "Type" => descending ? data.OrderByDescending(x => x.Type) : data.OrderBy(x => x.Type),
            "TicketId" => descending
                ? data.OrderByDescending(x => x.TicketId)
                : data.OrderBy(x => x.TicketId),
            _ => data,
        };
    }

    private static GridData<EventViewModel> Paginate(
        IEnumerable<EventViewModel> data,
        int page,
        int pageSize
    )
    {
        var materialized = data.ToList();

        return new GridData<EventViewModel>
        {
            TotalItems = materialized.Count,
            Items = materialized.Skip(page * pageSize).Take(pageSize).ToArray(),
        };
    }

    public Task<List<string>> GetVenuesAsync()
    {
        var venues = _events.Select(e => e.Venue).Distinct().OrderBy(v => v).ToList();
        return Task.FromResult(venues);
    }

    public Task<List<string>> GetEventTypesAsync()
    {
        var types = _events.Select(e => e.Type).Distinct().OrderBy(t => t).ToList();
        return Task.FromResult(types);
    }

    public Task<bool> CancelEventAsync(Guid eventId)
    {
        var removed = _events.RemoveAll(e => e.Id == eventId);
        return Task.FromResult(removed > 0);
    }
}
