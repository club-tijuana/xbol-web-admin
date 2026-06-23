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
        Assert.Contains("Validation=\"@ValidateRenewalEndDate\"", source, StringComparison.Ordinal);

        Assert.Contains("ValidateDateOrder(", source, StringComparison.Ordinal);
        Assert.Contains("ValidateRequiredDatePair(", source, StringComparison.Ordinal);
        Assert.Contains("ValidateRequiredTimePair(", source, StringComparison.Ordinal);
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
        Assert.Contains("catch (ApiException<ValidationProblemDetails> ex)", source, StringComparison.Ordinal);
        Assert.Contains("catch (ApiException ex) when (TryReadValidationProblemDetails(ex, out var problem))", source, StringComparison.Ordinal);
        Assert.Contains("Snackbar.Add(FormatMediaErrorToast(", source, StringComparison.Ordinal);
        Assert.Contains("L[\"ErrorSavingMedia\"]", source, StringComparison.Ordinal);
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
