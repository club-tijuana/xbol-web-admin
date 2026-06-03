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
        Assert.Contains("Navigation.NavigateTo(\"/events/bundles/basic/create\")", eventsSource, StringComparison.Ordinal);
        Assert.Contains("Navigation.NavigateTo(\"/events/bundles/season-pass/create\")", eventsSource, StringComparison.Ordinal);

        Assert.DoesNotContain("@page \"/events/bundles/create/{BundleKind}\"", bundleCreateSource, StringComparison.Ordinal);
        Assert.DoesNotContain("Navigation.NavigateTo(\"/events/bundles/create/basic\")", eventsSource, StringComparison.Ordinal);
        Assert.DoesNotContain("Navigation.NavigateTo(\"/events/bundles/create/season-pass\")", eventsSource, StringComparison.Ordinal);
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

    private static string ReadAppSource(string relativePath)
    {
        var path = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "../../../..",
            "Odasoft.XBOL.AdminPortal",
            relativePath));

        return File.ReadAllText(path);
    }
}
