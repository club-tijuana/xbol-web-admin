using MudBlazor;
using Odasoft.XBOL.AdminPortal.Components.Dialogs;

namespace Odasoft.XBOL.AdminPortal.Extensions;

public static class DialogServiceExtensions
{
    public static async Task<bool> ShowConfirmDeleteAsync(
        this IDialogService dialogService,
        string title,
        string message
    )
    {
        var parameters = new DialogParameters<ConfirmDeleteDialog>
        {
            { x => x.Title, title },
            { x => x.Message, message },
        };

        var dialog = await dialogService.ShowAsync<ConfirmDeleteDialog>(null, parameters);
        var result = await dialog.Result;

        return result is { Canceled: false };
    }
}
