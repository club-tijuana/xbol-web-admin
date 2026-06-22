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
    public void Bundle_events_editor_uses_explicit_lists_and_delta_persistence()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");
        var normalizedSource = source.Replace("\r\n", "\n", StringComparison.Ordinal);
        var service = ReadAppSource("Services/ApiEventService.cs");
        var serviceInterface = ReadAppSource("Services/IEventService.cs");

        Assert.Contains("@L[\"BundleLineup\"]", source, StringComparison.Ordinal);
        Assert.Contains("@L[\"AvailableEventsToAdd\"]", source, StringComparison.Ordinal);
        Assert.Contains("bundle-events-target", source, StringComparison.Ordinal);
        Assert.Contains("bundle-events-source", source, StringComparison.Ordinal);
        Assert.Contains("bundle-events-selected-list", source, StringComparison.Ordinal);
        Assert.True(
            source.IndexOf("@L[\"BundleLineup\"]", StringComparison.Ordinal) <
            source.IndexOf("@L[\"AvailableEventsToAdd\"]", StringComparison.Ordinal),
            "Selected bundle events should appear before available candidate events.");
        Assert.Contains("AddSelectedEvent(context)", source, StringComparison.Ordinal);
        Assert.Contains("RemoveSelectedEvent(item)", source, StringComparison.Ordinal);
        Assert.DoesNotContain("Items=\"@SelectedEvents\"", source, StringComparison.Ordinal);
        Assert.Contains("Items=\"@AvailableCandidateEvents\"", source, StringComparison.Ordinal);
        Assert.Contains("BundleEventSelectionState _eventSelection", source, StringComparison.Ordinal);
        Assert.Contains("LoadPersistedBundleEventsAsync(BundleId.Value)", source, StringComparison.Ordinal);
        Assert.Contains("SaveBundleEventSchedulesAsync(BundleId.Value)", source, StringComparison.Ordinal);
        Assert.Contains("EventService.RemoveBundleEventSchedulesAsync(bundleId, delta.RemovedEventScheduleIds)", source, StringComparison.Ordinal);
        Assert.Contains("EventService.AddBundleEventSchedulesAsync(bundleId, delta.AddedEventScheduleIds)", source, StringComparison.Ordinal);
        Assert.Contains("!_eventSelection.IsPersisted(scheduleId)", source, StringComparison.Ordinal);
        Assert.Contains("Disabled=\"@(!CanRemoveSelectedEvent(item))\"", source, StringComparison.Ordinal);
        Assert.Contains("GetDelta(skipRemovals: IsPublishedSeasonPass)", source, StringComparison.Ordinal);
        Assert.Contains("ScheduleStatus: e.Status", service, StringComparison.Ordinal);
        Assert.Contains("ScheduleStatus? ScheduleStatus", ReadAppSource("ViewModels/EventViewModel.cs"), StringComparison.Ordinal);
        Assert.DoesNotContain("<MudItem xs=\"12\" lg=\"7\">", source, StringComparison.Ordinal);
        Assert.DoesNotContain("<MudItem xs=\"12\" lg=\"5\">", source, StringComparison.Ordinal);
        Assert.DoesNotContain("_selectedEventsByScheduleId.Clear();\n        _persistedEventScheduleIds.Clear();", normalizedSource, StringComparison.Ordinal);
        Assert.DoesNotContain("_selectedEventsByScheduleId", source, StringComparison.Ordinal);
        Assert.DoesNotContain("_persistedEventScheduleIds", source, StringComparison.Ordinal);
        Assert.DoesNotContain("SelectedItems=\"_visibleSelectedEvents\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("SelectedItemsChanged=\"OnVisibleSelectedEventsChanged\"", source, StringComparison.Ordinal);

        Assert.Contains("Task AddBundleEventSchedulesAsync(long bundleId, IReadOnlyList<long> eventScheduleIds)", serviceInterface, StringComparison.Ordinal);
        Assert.Contains("Task RemoveBundleEventSchedulesAsync(long bundleId, IReadOnlyList<long> eventScheduleIds)", serviceInterface, StringComparison.Ordinal);
        Assert.Contains("adminClient.AddBundleEventSchedulesAsync", service, StringComparison.Ordinal);
        Assert.Contains("adminClient.RemoveBundleEventSchedulesAsync", service, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_price_editor_exposes_copy_to_all_and_alignment_class()
    {
        var source = ReadAppSource("Components/BundlePriceEditor.razor");
        var css = ReadAppSource("Components/BundlePriceEditor.razor.css");

        Assert.Contains("CopyPriceTypeToAll(context, DefaultPriceTypeName)", source, StringComparison.Ordinal);
        Assert.Contains("CanCopyPriceType(source, priceType)", source, StringComparison.Ordinal);
        Assert.Contains("Disabled=\"@(!CanCopyPriceType(context, DefaultPriceTypeName))\"", source, StringComparison.Ordinal);
        Assert.Contains("Disabled=\"@(!CanCopyPriceType(context, priceType))\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("StartIcon=\"@Icons.Material.Outlined.ContentCopy\"", source, StringComparison.Ordinal);
        Assert.Contains("aria-label=\"@L[\"CopyToAll\"]\"", ReadAppSource("Components/Shared/PriceCopyButton.razor"), StringComparison.Ordinal);
        Assert.Contains("Class=\"bundle-price-editor-table\"", source, StringComparison.Ordinal);
        Assert.Contains("Class=\"price-cell\"", source, StringComparison.Ordinal);
        Assert.Contains("class=\"price-copy-action\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("Immediate=\"@true\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("Immediate=\"@false\"", source, StringComparison.Ordinal);
        Assert.Contains("vertical-align: top", css, StringComparison.Ordinal);
        Assert.Contains(".price-copy-action", css, StringComparison.Ordinal);
        Assert.Contains("height: 40px", css, StringComparison.Ordinal);
        Assert.Contains("width: 40px", css, StringComparison.Ordinal);
        Assert.Contains("display: flex", css, StringComparison.Ordinal);
        Assert.Contains("align-items: center", css, StringComparison.Ordinal);
        Assert.DoesNotContain("112px", css, StringComparison.Ordinal);
        Assert.DoesNotContain("padding-top", css, StringComparison.Ordinal);
    }

    [Fact]
    public void Event_price_editor_exposes_always_visible_copy_to_all_and_alignment_class()
    {
        var source = ReadAppSource("Components/Pages/EventEdit.razor");
        var css = ReadAppSource("Components/Pages/EventEdit.razor.css");

        Assert.Contains("Class=\"event-price-editor-table\"", source, StringComparison.Ordinal);
        Assert.Contains("<MudTd Style=\"vertical-align: top;\"></MudTd>", source, StringComparison.Ordinal);
        Assert.Contains("<MudTd Style=\"vertical-align: top;\">", source, StringComparison.Ordinal);
        Assert.Contains("<MudTd DataLabel=\"@priceType.Key\" Style=\"vertical-align: top;\">", source, StringComparison.Ordinal);
        Assert.Contains("Class=\"price-cell\"", source, StringComparison.Ordinal);
        Assert.Contains("class=\"price-copy-action\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("Immediate=\"@true\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("Immediate=\"@false\"", source, StringComparison.Ordinal);
        Assert.Contains("CopyPriceTypeToAll(context, priceType.Key)", source, StringComparison.Ordinal);
        Assert.Contains("CanCopyPriceType(source, priceType)", source, StringComparison.Ordinal);
        Assert.Contains("<PriceCopyButton Disabled=\"@_isReview\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("Disabled=\"@(_isReview || !CanCopyPriceType", source, StringComparison.Ordinal);
        Assert.DoesNotContain("StartIcon=\"@Icons.Material.Outlined.ContentCopy\"", source, StringComparison.Ordinal);
        Assert.Contains("SetPriceTypeValue(price, priceType, value)", source, StringComparison.Ordinal);
        Assert.Contains("row.PriceTypes.TryAdd(key, PriceTypes.TryGetValue(key, out var isBasePrice) && isBasePrice)", source, StringComparison.Ordinal);
        Assert.Contains("vertical-align: top", css, StringComparison.Ordinal);
        Assert.Contains(".price-copy-action", css, StringComparison.Ordinal);
        Assert.Contains("height: 40px", css, StringComparison.Ordinal);
        Assert.Contains("width: 40px", css, StringComparison.Ordinal);
        Assert.Contains("display: flex", css, StringComparison.Ordinal);
        Assert.Contains("align-items: center", css, StringComparison.Ordinal);
        Assert.DoesNotContain("112px", css, StringComparison.Ordinal);
        Assert.DoesNotContain("padding-top", css, StringComparison.Ordinal);
    }

    [Fact]
    public void Price_copy_button_centralizes_copy_to_all_markup()
    {
        var source = ReadAppSource("Components/Shared/PriceCopyButton.razor");

        Assert.Contains("<MudTooltip Text=\"@L[\"CopyToAll\"]\"", source, StringComparison.Ordinal);
        Assert.Contains("<MudIconButton Icon=\"@Icons.Material.Outlined.ContentCopy\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("StartIcon=\"@Icons.Material.Outlined.ContentCopy\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("@L[\"CopyToAll\"]</MudButton>", source, StringComparison.Ordinal);
        Assert.Contains("Disabled=\"@Disabled\"", source, StringComparison.Ordinal);
        Assert.Contains("OnClick=\"OnClick\"", source, StringComparison.Ordinal);
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
