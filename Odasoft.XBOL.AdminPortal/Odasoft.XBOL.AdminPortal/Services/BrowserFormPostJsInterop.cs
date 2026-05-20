using Microsoft.JSInterop;

namespace Odasoft.XBOL.AdminPortal.Services;

public sealed class BrowserFormPostJsInterop(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private IJSObjectReference? _module;

    public async Task PostAsync(string action, IReadOnlyDictionary<string, string?> fields)
    {
        var module = await GetModuleAsync();
        await module.InvokeVoidAsync("postForm", action, fields);
    }

    private async Task<IJSObjectReference> GetModuleAsync()
    {
        _module ??= await jsRuntime.InvokeAsync<IJSObjectReference>(
            "import",
            "./js/formPost.js");

        return _module;
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            await _module.DisposeAsync();
        }
    }
}
