using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class BoxOfficeSeasonPassRegressionTests
{
    private static readonly string ProjectRoot = FindProjectRoot();

    [Fact]
    public void BoxOffice_blocks_unsellable_season_pass_navigation()
    {
        var source = ReadPortalFile("Components/Pages/BoxOffice.razor");

        Assert.Contains("SeasonPassPurchaseWindow.CanBuy", source, StringComparison.Ordinal);
        Assert.Contains("buyableOnly: true", source, StringComparison.Ordinal);
        Assert.Contains("Snackbar.Add", source, StringComparison.Ordinal);
        Assert.Contains("CannotBookSeasonPass", source, StringComparison.Ordinal);
        Assert.Contains("return;", source, StringComparison.Ordinal);
    }

    [Fact]
    public void BoxOfficeSeasonCheckout_surfaces_api_validation_errors()
    {
        var source = ReadPortalFile("Components/Pages/BoxOfficeSeasonCheckout.razor");

        Assert.Contains("catch (ApiException apiEx)", source, StringComparison.Ordinal);
        Assert.Contains("LogBookingApiException(apiEx", source, StringComparison.Ordinal);
        Assert.Contains("GetApiErrorMessage(apiEx.Response)", source, StringComparison.Ordinal);
        Assert.Contains("parsed.Value<string>(\"detail\")", source, StringComparison.Ordinal);
        Assert.Contains("parsed.Value<string>(\"title\")", source, StringComparison.Ordinal);
    }

    private static string ReadPortalFile(string relativePath)
    {
        var fullPath = Path.Combine(ProjectRoot, "Odasoft.XBOL.AdminPortal", relativePath);
        return File.ReadAllText(fullPath);
    }

    private static string FindProjectRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(
                directory.FullName,
                "Odasoft.XBOL.AdminPortal",
                "Odasoft.XBOL.AdminPortal.csproj");

            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate Odasoft.XBOL.AdminPortal project root.");
    }
}
