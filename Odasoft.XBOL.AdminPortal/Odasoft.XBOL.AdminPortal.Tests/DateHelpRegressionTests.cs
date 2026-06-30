using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class DateHelpRegressionTests
{
    [Fact]
    public void Event_edit_dates_step_exposes_current_behavior_help()
    {
        var source = ReadAppSource("Components/Pages/EventEdit.razor");

        Assert.Contains("@L[\"EventDatesScheduleHelp\"]", source, StringComparison.Ordinal);
        Assert.Contains("@L[\"EventDatesPreSaleHelp\"]", source, StringComparison.Ordinal);
        Assert.Contains("@L[\"EventDatesSaleHelp\"]", source, StringComparison.Ordinal);
        Assert.Contains("@L[\"EventDatesPublishingHelp\"]", source, StringComparison.Ordinal);
        Assert.Contains("@L[\"EventDatesAccessHelp\"]", source, StringComparison.Ordinal);
        Assert.Contains("<DateTimelineHelpButton Title=\"@L[\"EventDatesTimelineTitle\"]\"", source, StringComparison.Ordinal);
        Assert.Contains("Tooltip=\"@L[\"EventDatesTimelineTooltip\"]\"", source, StringComparison.Ordinal);
        Assert.Contains("Items=\"EventTimelineSteps\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("<DateTimelineHelp Overview=\"@L[\"EventDatesTimelineOverview\"]\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("OpenEventDatesHelpAsync", source, StringComparison.Ordinal);
        Assert.DoesNotContain("DateBehaviorHelpDialog", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_create_dates_step_exposes_current_behavior_help()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("@L[\"BundleDatesScheduleHelp\"]", source, StringComparison.Ordinal);
        Assert.Contains("@L[\"BundleDatesSaleHelp\"]", source, StringComparison.Ordinal);
        Assert.Contains("@L[\"BundleDatesPublishingHelp\"]", source, StringComparison.Ordinal);
        Assert.Contains("@L[\"BundleDatesRenewalHelp\"]", source, StringComparison.Ordinal);
        Assert.Contains("<DateTimelineHelpButton Title=\"@L[\"BundleDatesTimelineTitle\"]\"", source, StringComparison.Ordinal);
        Assert.Contains("Tooltip=\"@L[\"BundleDatesTimelineTooltip\"]\"", source, StringComparison.Ordinal);
        Assert.Contains("Items=\"BundleTimelineSteps\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("<DateTimelineHelp Overview=\"@L[\"BundleDatesTimelineOverview\"]\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("OpenBundleDatesHelpAsync", source, StringComparison.Ordinal);
        Assert.DoesNotContain("DateBehaviorHelpDialog", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Shared_date_timeline_help_component_uses_mud_timeline()
    {
        var source = ReadAppSource("Components/Shared/DateTimelineHelp.razor");

        Assert.Contains("Overview", source, StringComparison.Ordinal);
        Assert.Contains("<MudTimeline", source, StringComparison.Ordinal);
        Assert.Contains("<MudTimelineItem", source, StringComparison.Ordinal);
        Assert.Contains("DateTimelineStep", source, StringComparison.Ordinal);
        Assert.DoesNotContain("MudChip", source, StringComparison.Ordinal);
        Assert.DoesNotContain("Icons.Material.Filled.ChevronRight", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Shared_date_timeline_help_button_opens_dialog()
    {
        var source = ReadAppSource("Components/Shared/DateTimelineHelpButton.razor");

        Assert.Contains("@inject IDialogService DialogService", source, StringComparison.Ordinal);
        Assert.Contains("<MudTooltip Text=\"@Tooltip\"", source, StringComparison.Ordinal);
        Assert.Contains("Icons.Material.Outlined.Info", source, StringComparison.Ordinal);
        Assert.Contains("ShowAsync<DateTimelineHelpDialog>", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Date_timeline_help_dialog_hosts_shared_timeline()
    {
        var source = ReadAppSource("Components/Dialogs/DateTimelineHelpDialog.razor");

        Assert.Contains("<MudDialog", source, StringComparison.Ordinal);
        Assert.Contains("<DateTimelineHelp Overview=\"@Overview\" Items=\"Items\" />", source, StringComparison.Ordinal);
        Assert.Contains("[CascadingParameter] IMudDialogInstance MudDialog", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_create_renewal_help_is_limited_to_season_pass_branch()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");
        var seasonPassBranchStart = source.IndexOf("@if (_bundleType == BundleType.SeasonPass)", StringComparison.Ordinal);

        Assert.True(seasonPassBranchStart >= 0);
        Assert.DoesNotContain("@L[\"BundleDatesRenewalHelp\"]", source[..seasonPassBranchStart], StringComparison.Ordinal);
        Assert.Contains("@L[\"BundleDatesRenewalHelp\"]", source[seasonPassBranchStart..], StringComparison.Ordinal);
    }

    [Fact]
    public void Date_help_tests_read_only_repo_files()
    {
        var source = ReadTestSource();

        Assert.DoesNotContain("Read" + "WorkspaceDoc", source, StringComparison.Ordinal);
        Assert.DoesNotContain("../../../" + "../../..", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Date_behavior_help_dialog_was_removed()
    {
        var path = Path.Combine(GetAppSourceRoot(), "Components/Dialogs/DateBehaviorHelpDialog.razor");

        Assert.False(File.Exists(path));
    }

    private static string ReadAppSource(string relativePath)
    {
        var path = Path.Combine(GetAppSourceRoot(), relativePath);

        return File.ReadAllText(path);
    }

    private static string ReadTestSource()
    {
        var path = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "../../../..",
            "Odasoft.XBOL.AdminPortal.Tests",
            "DateHelpRegressionTests.cs"));

        return File.ReadAllText(path);
    }

    private static string GetAppSourceRoot()
    {
        return Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "../../../..",
            "Odasoft.XBOL.AdminPortal"));
    }
}
