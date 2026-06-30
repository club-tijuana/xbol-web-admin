using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class BundleCreateValidationRegressionTests
{
    [Fact]
    public void Bundle_create_date_fields_have_local_validation_hooks()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("Validation=\"@ValidateStartDate\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@(new Func<TimeSpan?, IEnumerable<string>>(ValidateStartTime))\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@ValidateEndDate\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@(new Func<TimeSpan?, IEnumerable<string>>(ValidateEndTime))\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@ValidateOnSaleDate\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@(new Func<TimeSpan?, IEnumerable<string>>(ValidateOnSaleTime))\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@ValidateOffSaleDate\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@(new Func<TimeSpan?, IEnumerable<string>>(ValidateOffSaleTime))\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@ValidatePreSaleDate\"", source, StringComparison.Ordinal);
        Assert.Contains("ValidatePreviousBundleId", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@ValidateRenewalStartDate\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@(new Func<TimeSpan?, IEnumerable<string>>(ValidateRenewalStartTime))\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@ValidateRenewalEndDate\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@(new Func<TimeSpan?, IEnumerable<string>>(ValidateRenewalEndTime))\"", source, StringComparison.Ordinal);

        Assert.Contains("ValidateDateOrder(", source, StringComparison.Ordinal);
        Assert.Contains("ValidateDateOrderOrEqual(", source, StringComparison.Ordinal);
        Assert.Contains("ValidateRequiredDatePair(", source, StringComparison.Ordinal);
        Assert.Contains("ValidateRequiredTimePair(", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_create_renewal_window_validation_matches_ticketing_rules()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("private bool IsRenewalBundle", source, StringComparison.Ordinal);
        Assert.Contains("private bool HasRenewalWindowInput", source, StringComparison.Ordinal);
        Assert.Contains("ValidateRenewalWindowAllowed()", source, StringComparison.Ordinal);
        Assert.Contains("ValidateRenewalWindowRequired(value)", source, StringComparison.Ordinal);
        Assert.Contains("L[\"BundleRenewalStartRequired\"].Value", source, StringComparison.Ordinal);
        Assert.Contains("L[\"BundleRenewalEndRequired\"].Value", source, StringComparison.Ordinal);
        Assert.Contains("L[\"BundleRenewalDatesRequirePreviousBundle\"].Value", source, StringComparison.Ordinal);
        Assert.Contains("L[\"PreSaleAfterRenewalEnd\"].Value", source, StringComparison.Ordinal);
        Assert.Contains("L[\"OnSaleAfterRenewalEnd\"].Value", source, StringComparison.Ordinal);
        Assert.Contains("AddDateValidationErrors(\"PreviousBundleId\", ValidatePreviousBundleId(_previousBundleId));", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_create_save_validates_all_relevant_steps_before_returning()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("private async Task<BundleSubmitValidationResult> ValidateAllStepsForSubmitAsync()", source, StringComparison.Ordinal);
        Assert.Contains("var validation = await ValidateAllStepsForSubmitAsync();", source, StringComparison.Ordinal);
        Assert.Contains("_activeIndex = validation.FirstInvalidStepIndex;", source, StringComparison.Ordinal);
        Assert.Contains("ValidateStepAsync(0)", source, StringComparison.Ordinal);
        Assert.Contains("ValidateDateFieldsForSubmit()", source, StringComparison.Ordinal);
        Assert.Contains("ValidateStepAsync(EventsStepIndex)", source, StringComparison.Ordinal);
        Assert.Contains("_validateActiveStepAfterRender = true;", source, StringComparison.Ordinal);

        Assert.DoesNotContain("if (!await ValidateStepAsync(EventsStepIndex))", source, StringComparison.Ordinal);
        Assert.DoesNotContain("if (!await ValidateStepAsync(0))", source, StringComparison.Ordinal);
        Assert.DoesNotContain("if (!await ValidateStepAsync(DatesStepIndex))", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_create_date_submit_validation_collects_all_date_errors()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");
        var normalizedSource = source.Replace("\r\n", "\n", StringComparison.Ordinal);

        Assert.Contains("private readonly Dictionary<string, List<string>> _dateValidationErrors = new(StringComparer.OrdinalIgnoreCase);", source, StringComparison.Ordinal);
        Assert.Contains("AddDateValidationErrors(", source, StringComparison.Ordinal);
        Assert.Contains("_datesFormValid = _dateValidationErrors.Count == 0;", source, StringComparison.Ordinal);
        Assert.DoesNotContain("var hasErrors =\n            ValidateStartDate(_startDate).Any() ||", normalizedSource, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_create_time_fields_have_explicit_validation_delegates()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("Validation=\"@(new Func<TimeSpan?, IEnumerable<string>>(ValidateStartTime))\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@(new Func<TimeSpan?, IEnumerable<string>>(ValidateEndTime))\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@(new Func<TimeSpan?, IEnumerable<string>>(ValidateOnSaleTime))\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@(new Func<TimeSpan?, IEnumerable<string>>(ValidateOffSaleTime))\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@(new Func<TimeSpan?, IEnumerable<string>>(ValidateRenewalStartTime))\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@(new Func<TimeSpan?, IEnumerable<string>>(ValidateRenewalEndTime))\"", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_edit_uses_loaded_bundle_name_as_page_title()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("_pageTitle = string.IsNullOrWhiteSpace(bundleResult.Name)", source, StringComparison.Ordinal);
        Assert.Contains(": bundleResult.Name;", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Season_pass_bundle_create_exposes_previous_bundle_selector()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("_previousSeasonPassBundles", source, StringComparison.Ordinal);
        Assert.Contains("@bind-Value=\"_previousBundleId\"", source, StringComparison.Ordinal);
        Assert.Contains("PreviousBundleId = _previousBundleId", source, StringComparison.Ordinal);
        Assert.Contains("_previousBundleId = bundleResult.PreviousBundleId;", source, StringComparison.Ordinal);
        Assert.Contains("GetSeasonPassBundlesAsync(", source, StringComparison.Ordinal);
        Assert.Contains("BundleType.SeasonPass", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_edit_save_navigates_to_bundle_detail()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("Navigation.NavigateTo(GetBundleSaveSuccessRoute());", source, StringComparison.Ordinal);
        Assert.Contains("private string GetBundleSaveSuccessRoute()", source, StringComparison.Ordinal);
        Assert.Contains("if (_isEdit && BundleId is long bundleId)", source, StringComparison.Ordinal);
        Assert.Contains("$\"./events/bundles/{bundleId}\"", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_submit_validation_handles_unmounted_step_forms()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("private async Task<bool> ValidatePriceStepAsync()", source, StringComparison.Ordinal);
        Assert.Contains("var priceValidation = await _pricesValidator.ValidateAsync(BuildPricesInfoModel());", source, StringComparison.Ordinal);
        Assert.Contains("private async Task<bool> ValidateAdditionalChargesStepAsync()", source, StringComparison.Ordinal);
        Assert.Contains("var additionalChargesValidation = await _additionalChargesInfoValidator.ValidateAsync(_additionalCharges);", source, StringComparison.Ordinal);
        Assert.Contains("private PricesInfoModel BuildPricesInfoModel()", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_create_preview_validation_does_not_cancel_step_navigation()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("private async Task OnPreviewInteraction(StepperInteractionEventArgs arg)", source, StringComparison.Ordinal);
        Assert.DoesNotContain("arg.Cancel", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_create_validation_problem_details_are_mapped_to_field_errors()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("catch (ApiException<ValidationProblemDetails> ex)", source, StringComparison.Ordinal);
        Assert.Contains("ApplyValidationProblemDetails(ex.Result);", source, StringComparison.Ordinal);
        Assert.Contains("_datesFormContext.HandleValidationProblemDetails(problem);", source, StringComparison.Ordinal);
        Assert.Contains("_descriptionFormContext.HandleValidationProblemDetails(problem);", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_create_media_errors_are_shown_on_images_step()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("HasError=\"ImagesStepHasError\"", source, StringComparison.Ordinal);
        Assert.Contains("ImagesStepHasError => _imagesStepValidationAttempted && !_imagesStepValid", source, StringComparison.Ordinal);
        Assert.Contains("_imageValidationErrors", source, StringComparison.Ordinal);
        Assert.Contains("_activeIndex = ImagesStepIndex;", source, StringComparison.Ordinal);
        Assert.Contains("HandleMediaValidationFailure", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_create_rejects_too_small_images_before_upload()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("private const int MinImageWidth = 1920;", source, StringComparison.Ordinal);
        Assert.Contains("private const int MinImageHeight = 1080;", source, StringComparison.Ordinal);
        Assert.Contains("imageInfo.Width < MinImageWidth || imageInfo.Height < MinImageHeight", source, StringComparison.Ordinal);
        Assert.Contains("L[\"ImageTooSmall\", file.Name, imageInfo.Width, imageInfo.Height, MinImageWidth, MinImageHeight]", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_create_media_upload_failure_uses_media_specific_message()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("if (!await TrySaveBundleMediaAsync(BundleId.Value))", source, StringComparison.Ordinal);
        Assert.Contains("await SaveBundleMediaAsync(bundleId);", source, StringComparison.Ordinal);
        Assert.Contains("new List<BundleMediaDesiredItem>();", source, StringComparison.Ordinal);
        Assert.Contains("AddDesiredBundleMediaItem(items, files, streams, _bannerImage, AdminMediaType.Banner, 0);", source, StringComparison.Ordinal);
        Assert.Contains("ExistingMediaId = image.Id.Value", source, StringComparison.Ordinal);
        Assert.Contains("FileIndex = fileIndex", source, StringComparison.Ordinal);
        Assert.Contains("await MediaService.ReconcileBundleMediaAsync(bundleId, items, files);", source, StringComparison.Ordinal);
        Assert.Contains("ApplyBundleMediaResponse(media);", source, StringComparison.Ordinal);
        Assert.DoesNotContain("UploadBundleMediaItemAsync", source, StringComparison.Ordinal);
        Assert.DoesNotContain("MediaService.UploadMediaAsync(bundleId, AdminSaleType.Bundle", source, StringComparison.Ordinal);
        Assert.Contains("catch (ApiException<ValidationProblemDetails> ex)", source, StringComparison.Ordinal);
        Assert.Contains("catch (ApiException ex) when (TryReadValidationProblemDetails(ex, out var problem))", source, StringComparison.Ordinal);
        Assert.Contains("Snackbar.Add(FormatMediaErrorToast(", source, StringComparison.Ordinal);
        Assert.Contains("L[\"ErrorSavingMedia\"]", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_create_image_uploads_show_processing_feedback()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("_processingImages", source, StringComparison.Ordinal);
        Assert.Contains("_savingBundleMedia", source, StringComparison.Ordinal);
        Assert.Contains("MudProgressLinear Indeterminate", source, StringComparison.Ordinal);
        Assert.Contains("L[\"ProcessingImages\"]", source, StringComparison.Ordinal);
        Assert.Contains("L[\"SavingImages\"]", source, StringComparison.Ordinal);
        Assert.Contains("Disabled=\"@ImagesBusy\"", source, StringComparison.Ordinal);
        Assert.Contains("Disabled=\"@(_isReview || ImagesBusy)\"", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_create_banner_chip_uses_stable_image_reference()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("@if (_bannerImage is { } bannerImage)", source, StringComparison.Ordinal);
        Assert.Contains("@bannerImage.Name", source, StringComparison.Ordinal);
        Assert.DoesNotContain("@_bannerImage.Name", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_create_empty_event_selection_remains_valid()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("if (_eventSelection.Count == 0)", source, StringComparison.Ordinal);
        Assert.Contains("return true;", source, StringComparison.Ordinal);
        Assert.DoesNotContain("_selectedEventsByScheduleId.Count == 0)\n        {\n            return false;", source, StringComparison.Ordinal);
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
}
