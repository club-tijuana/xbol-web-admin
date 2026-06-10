using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Odasoft.XBOL.Common.Options;

namespace Odasoft.XBOL.AdminPortal.Services;

public sealed class FirebaseAuthJsInterop(
    IJSRuntime jsRuntime,
    IOptions<FirebaseAuthOptions> options) : IAsyncDisposable
{
    private IJSObjectReference? _module;

    public async Task InitializeAsync()
    {
        var module = await GetModuleAsync();
        await module.InvokeVoidAsync("initializeFirebaseAuth", options.Value);
    }

    public async Task<FirebaseAuthUser?> GetCurrentUserAsync(bool forceRefresh = false)
    {
        await InitializeAsync();
        var module = await GetModuleAsync();
        return await module.InvokeAsync<FirebaseAuthUser?>("getCurrentUser", forceRefresh);
    }

    public async Task<FirebaseAuthUser> SignInAsync(string email, string password)
    {
        await InitializeAsync();
        var module = await GetModuleAsync();
        return await module.InvokeAsync<FirebaseAuthUser>("signIn", email, password);
    }

    public async Task SignOutAsync()
    {
        await InitializeAsync();
        var module = await GetModuleAsync();
        await module.InvokeVoidAsync("signOutUser");
    }

    public async Task<string> VerifyPasswordResetAsync(string oobCode)
    {
        await InitializeAsync();
        var module = await GetModuleAsync();
        return await module.InvokeAsync<string>("verifyPasswordReset", oobCode);
    }

    public async Task ConfirmPasswordResetAsync(string oobCode, string newPassword)
    {
        await InitializeAsync();
        var module = await GetModuleAsync();
        await module.InvokeVoidAsync("confirmPasswordResetCode", oobCode, newPassword);
    }

    private async Task<IJSObjectReference> GetModuleAsync()
    {
        _module ??= await jsRuntime.InvokeAsync<IJSObjectReference>(
            "import",
            "./js/firebaseAuth.js");

        return _module;
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            try
            {
                await _module.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // The client disconnected, so the JS runtime is gone.
                // We can safely ignore this.
            }
            catch (TaskCanceledException)
            {
                // The circuit is being torn down.
                // Also safe to ignore during disposal.
            }
        }
    }
}
