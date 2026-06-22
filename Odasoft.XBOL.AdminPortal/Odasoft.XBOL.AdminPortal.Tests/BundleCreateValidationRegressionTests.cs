using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class BundleCreateValidationRegressionTests
{
    [Fact]
    public void Bundle_create_date_fields_have_local_validation_hooks()
    {
        var source = ReadAppSource("Components/Pages/BundleCreate.razor");

        Assert.Contains("Validation=\"@ValidateStartDate\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@ValidateStartTime\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@ValidateEndDate\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@ValidateEndTime\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@ValidateOnSaleDate\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@ValidateOnSaleTime\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@ValidateOffSaleDate\"", source, StringComparison.Ordinal);
        Assert.Contains("Validation=\"@ValidateOffSaleTime\"", source, StringComparison.Ordinal);
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
