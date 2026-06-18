using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class BundleRouteRegressionTests
{
    [Fact]
    public void Bundle_create_uses_kind_before_create_in_route_and_menu_links()
    {
        var bundleCreateSource = ReadAppSource("Components/Pages/BundleCreate.razor");
        var eventsSource = ReadAppSource("Components/Pages/Events.razor");

        Assert.Contains("@page \"/events/bundles/{BundleKind}/create\"", bundleCreateSource, StringComparison.Ordinal);
        Assert.Contains("Navigation.NavigateTo(\"./events/bundles/basic/create\")", eventsSource, StringComparison.Ordinal);
        Assert.Contains("Navigation.NavigateTo(\"./events/bundles/season-pass/create\")", eventsSource, StringComparison.Ordinal);

        Assert.DoesNotContain("@page \"/events/bundles/create/{BundleKind}\"", bundleCreateSource, StringComparison.Ordinal);
        Assert.DoesNotContain("Navigation.NavigateTo(\"/events/bundles/create/basic\")", eventsSource, StringComparison.Ordinal);
        Assert.DoesNotContain("Navigation.NavigateTo(\"/events/bundles/create/season-pass\")", eventsSource, StringComparison.Ordinal);
    }

    [Fact]
    public void Season_pass_page_is_selected_bundle_route_without_inline_bundle_selector()
    {
        var source = ReadAppSource("Components/Pages/SeasonPass.razor");

        Assert.Contains("@page \"/season-pass/{BundleId:long}\"", source, StringComparison.Ordinal);
        Assert.Contains("[Parameter] public long? BundleId", source, StringComparison.Ordinal);
        Assert.Contains("AdminSaleType.Bundle", source, StringComparison.Ordinal);
        Assert.DoesNotContain("Label=\"Xolopass\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("Seleccione un Xolopass", source, StringComparison.Ordinal);
        Assert.DoesNotContain("Ver modo renovaci\u00f3n", source, StringComparison.Ordinal);
        Assert.DoesNotContain("_renewalMode", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Nav_menu_lists_published_season_pass_bundles_under_season_passes()
    {
        var source = ReadAppSource("Components/Layout/NavMenu.razor");

        Assert.Contains("IAdminClient AdminClient", source, StringComparison.Ordinal);
        Assert.Contains("GetBundlesAsync(", source, StringComparison.Ordinal);
        Assert.Contains("BundleType.SeasonPass", source, StringComparison.Ordinal);
        Assert.Contains("_localizer[\"SeasonPasses\"]", source, StringComparison.Ordinal);
        Assert.Contains("_localizer[\"SalesConfiguration\"]", source, StringComparison.Ordinal);
        Assert.Contains("@bundle.Name", source, StringComparison.Ordinal);
        Assert.Contains("./season-pass/{bundle.Id}", source, StringComparison.Ordinal);
        Assert.DoesNotContain("NoSeasonPasses", source, StringComparison.Ordinal);
        Assert.DoesNotContain("Xolopass", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Season_pass_checkout_go_back_returns_to_selected_season_pass_page()
    {
        var source = ReadAppSource("Components/Pages/Booking.razor");

        Assert.Contains("IsSeasonPassRoute", source, StringComparison.Ordinal);
        Assert.Contains("Navigation.NavigateTo($\"./season-pass/{Id}\")", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Admin_component_navigation_targets_are_base_relative()
    {
        var componentRoot = Path.Combine(GetAppSourceRoot(), "Components");
        var unsafeTargets = Directory.EnumerateFiles(componentRoot, "*.razor", SearchOption.AllDirectories)
            .SelectMany(FindRootRelativeNavigationTargets)
            .ToList();

        Assert.True(
            unsafeTargets.Count == 0,
            "Found root-relative component navigation targets that bypass Hosting:PathBase:"
            + Environment.NewLine
            + string.Join(Environment.NewLine, unsafeTargets));
    }

    [Fact]
    public void Bundle_create_stepper_exposes_step_validation_state()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("ActiveIndexChanged=\"HandleStepNavigation\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("_highestUnlockedStep", source, StringComparison.Ordinal);

        Assert.Contains("HasError=\"DescriptionStepHasError\"", source, StringComparison.Ordinal);
        Assert.Contains("@bind-Completed=\"_descriptionFormCompleted\"", source, StringComparison.Ordinal);
        Assert.Contains("HasError=\"PriceStepHasError\"", source, StringComparison.Ordinal);
        Assert.Contains("@bind-Completed=\"_pricesFormCompleted\"", source, StringComparison.Ordinal);
        Assert.Contains("HasError=\"DatesStepHasError\"", source, StringComparison.Ordinal);
        Assert.Contains("@bind-Completed=\"_datesFormCompleted\"", source, StringComparison.Ordinal);
        Assert.Contains("@bind-Completed=\"_imagesStepCompleted\"", source, StringComparison.Ordinal);
        Assert.Contains("HasError=\"EventsStepHasError\"", source, StringComparison.Ordinal);
        Assert.Contains("@bind-Completed=\"_eventsFormCompleted\"", source, StringComparison.Ordinal);

        Assert.Contains("DescriptionStepHasError => _descriptionFormValidationAttempted && !_descriptionFormValid", source, StringComparison.Ordinal);
        Assert.Contains("PriceStepHasError => _pricesFormValidationAttempted && !_pricesFormValid", source, StringComparison.Ordinal);
        Assert.Contains("DatesStepHasError => _datesFormValidationAttempted && !_datesFormValid", source, StringComparison.Ordinal);
        Assert.Contains("EventsStepHasError => _eventsFormValidationAttempted && !_eventsFormValid", source, StringComparison.Ordinal);

        Assert.DoesNotContain("HasError=\"!_descriptionFormValid\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("HasError=\"!_pricesFormValid\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("HasError=\"!_datesFormValid\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("HasError=\"!_eventsFormValid\"", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_create_images_use_event_caratula_and_gallery_without_logo_upload()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("@L[\"EventBanner\"]", source, StringComparison.Ordinal);
        Assert.Contains("@L[\"WebsiteGallery\"]", source, StringComparison.Ordinal);
        Assert.Contains("FilesChanged=\"UploadGalleryImagesAsync\"", source, StringComparison.Ordinal);
        Assert.Contains("AdminMediaType.Gallery", source, StringComparison.Ordinal);
        Assert.True(
            source.Split("@onclick:stopPropagation=\"true\"", StringSplitOptions.None).Length - 1 >= 2,
            "Gallery remove and clear controls should not reopen the file picker.");

        Assert.DoesNotContain("_logoImage", source, StringComparison.Ordinal);
        Assert.DoesNotContain("_logoImageUpload", source, StringComparison.Ordinal);
        Assert.DoesNotContain("AdminMediaType.Logo", source, StringComparison.Ordinal);
        Assert.DoesNotContain("BundleLogo", source, StringComparison.Ordinal);
        Assert.DoesNotContain("UploadLogo", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Catalog_routes_pending_review_to_review_and_changes_requested_to_edit()
    {
        var source = ReadAppSource("Components/Pages/Events.razor");

        Assert.Contains("item.Status == AdminEventStatus.PendingReview", source, StringComparison.Ordinal);
        Assert.Contains("item.Status == AdminEventStatus.ChangesRequested", source, StringComparison.Ordinal);
        Assert.Contains("\"/review\"", source, StringComparison.Ordinal);
        Assert.Contains("\"/edit\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("item.Status != AdminEventStatus.Published ? \"/review\"", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Review_dialog_uses_explicit_bundle_workflow_endpoints()
    {
        var source = ReadAppSource("Components/Dialogs/ReviewEventDialog.razor");

        Assert.Contains("ApproveBundleAsync(EventId)", source, StringComparison.Ordinal);
        Assert.Contains("RejectBundleAsync(EventId)", source, StringComparison.Ordinal);
        Assert.DoesNotContain("UpdateBundleAsync(EventId, new BundleUpdateRequest()", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_create_uses_explicit_workflow_endpoints()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("SubmitBundleAsync(BundleId.Value)", source, StringComparison.Ordinal);
        Assert.Contains("ResubmitBundleAsync(BundleId.Value)", source, StringComparison.Ordinal);
        Assert.Contains("PublishBundleAsync(BundleId.Value)", source, StringComparison.Ordinal);
        Assert.DoesNotContain("SaveBundleAsync(AdminEventStatus.PendingReview)", source, StringComparison.Ordinal);
        Assert.DoesNotContain("UpdateBundleAsync(BundleId.Value, new BundleUpdateRequest() { Status", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_edit_load_preserves_selected_categories_and_saved_prices()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("values?.Any(category => category.Id.HasValue) != true", source, StringComparison.Ordinal);
        Assert.Contains("SelectBundleCategories(bundleResult.Categories)", source, StringComparison.Ordinal);
        Assert.Contains("if (!_eventCategories.Any(category => category.Id == bundleCategory.Id))", source, StringComparison.Ordinal);
        Assert.Contains("_loadingBundleDataForEdit = true", source, StringComparison.Ordinal);
        Assert.Contains("shouldInitializePrices && !_loadingBundleDataForEdit", source, StringComparison.Ordinal);
        Assert.Contains("if (_selectedVenueMap is not null && _venueMapLayout is null)", source, StringComparison.Ordinal);
        Assert.Contains("await EnsurePriceRowsLoadedAsync()", source, StringComparison.Ordinal);
        Assert.Contains("if (_selectedVenueMap?.Id != venueMapId)", source, StringComparison.Ordinal);
        Assert.Contains("initializePrices && _prices.Count == 0", source, StringComparison.Ordinal);
        Assert.DoesNotContain("_eventCategories = bundleResult.Categories?.ToList() ?? []", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Event_edit_resubmits_changes_requested_events_after_save()
    {
        var source = ReadAppSource("Components/Pages/EventEdit.razor");

        Assert.Contains("_eventStatus == AdminEventStatus.ChangesRequested", source, StringComparison.Ordinal);
        Assert.Contains("ResubmitEventAsync(EventId.Value)", source, StringComparison.Ordinal);
    }

    private static string ReadAppSource(string relativePath)
    {
        var path = Path.Combine(GetAppSourceRoot(), relativePath);

        return File.ReadAllText(path);
    }

    private static string GetAppSourceRoot()
    {
        return Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "../../../..",
            "Odasoft.XBOL.AdminPortal"));
    }

    private static IEnumerable<string> FindRootRelativeNavigationTargets(string path)
    {
        var relativePath = Path.GetRelativePath(GetAppSourceRoot(), path);
        var lines = File.ReadLines(path);
        var lineNumber = 0;

        foreach (var line in lines)
        {
            lineNumber++;

            if (System.Text.RegularExpressions.Regex.IsMatch(line, @"NavigateTo\([^;\r\n]*[$@]?""/")
                || System.Text.RegularExpressions.Regex.IsMatch(line, @"\bHref\s*=\s*[$@]?""/")
                || System.Text.RegularExpressions.Regex.IsMatch(line, @"\bBackUrl\s*=\s*[$@]?""/"))
            {
                yield return $"{relativePath}:{lineNumber}: {line.Trim()}";
            }
        }
    }
}
