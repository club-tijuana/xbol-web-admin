using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Odasoft.XBOL.AdminPortal.Components;

public abstract class SeatsioChartBase : ComponentBase, IAsyncDisposable
{
    [Inject] protected IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter][EditorRequired] public string SecretKey { get; set; } = default!;
    [Parameter][EditorRequired] public string EventKey { get; set; } = default!;
    [Parameter][EditorRequired] public long EventId { get; set; }
    [Parameter] public string Language { get; set; } = "es";

    protected abstract string ChartId { get; }

    protected IJSObjectReference? _module;
    protected IJSObjectReference? _chart;

    public async ValueTask FocusSeatAsync(string seatId)
    {
        if (_module is not null && _chart is not null)
        {
            await _module.InvokeVoidAsync("focusSeat", _chart, seatId);
        }
    }

    public async ValueTask UnfocusSeatAsync(string seatId)
    {
        if (_module is not null && _chart is not null)
        {
            await _module.InvokeVoidAsync("unfocusSeat", _chart, seatId);
        }
    }

    public async ValueTask DeselectSeatAsync(string seatId)
    {
        if (_module is not null && _chart is not null)
        {
            await _module.InvokeVoidAsync("deselectSeat", _chart, seatId);
        }
    }


    public async ValueTask ClearSelection()
    {
        if (_module is not null && _chart is not null)
        {
            await _module.InvokeVoidAsync("clearSelection", _chart);
        }
    }

    public async ValueTask ChangeConfig(object config)
    {
        if (_module is not null && _chart is not null)
        {
            await _module.InvokeVoidAsync("changeConfig", _chart, config);
        }
    }

    public async ValueTask FocusFilteredCategories()
    {
        if (_module is not null && _chart is not null)
        {
            await _module.InvokeVoidAsync("focusFilteredCategories", _chart);
        }
    }

    public virtual async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        if (_module is not null && _chart is not null)
        {
            try { await _module.InvokeVoidAsync("destroyChart", _chart); }
            catch (JSDisconnectedException) { }
        }

        if (_module is not null)
        {
            try { await _module.DisposeAsync(); }
            catch (JSDisconnectedException) { }
        }
    }
}
