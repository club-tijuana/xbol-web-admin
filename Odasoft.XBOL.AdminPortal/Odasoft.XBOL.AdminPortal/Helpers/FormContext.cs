using MudBlazor;
using Newtonsoft.Json.Linq;
using Odasoft.XBOL.Business;

namespace Odasoft.XBOL.AdminPortal.Helpers;

public class FormContext
{
    private readonly Dictionary<string, List<string>> _serverErrors = new(StringComparer.OrdinalIgnoreCase);
    private readonly Action _stateHasChanged;
    private MudForm? _form;

    public FormContext(Action stateHasChanged)
    {
        _stateHasChanged = stateHasChanged;
    }

    public bool IsSaving { get; private set; }

    public FieldBinding Field(string fieldName) => new(fieldName, this);

    public Func<T, IEnumerable<string>> ValidationFor<T>(string fieldName) =>
        _ => _serverErrors.TryGetValue(fieldName, out var errors) ? errors : Enumerable.Empty<string>();

    public void ClearFieldError(string fieldName)
    {
        if (_serverErrors.Remove(fieldName))
        {
            _stateHasChanged();
        }
    }

    internal void SetForm(MudForm form) => _form = form;

    internal void SetSaving(bool saving) => IsSaving = saving;

    internal void HandleApiException(ApiException<ProblemDetails> ex)
    {
        _serverErrors.Clear();

        LoadErrorsFromAdditionalProperties(ex.Result.AdditionalProperties);

        _form?.ValidateAsync();
    }

    internal void HandleValidationProblemDetails(ValidationProblemDetails problem)
    {
        _serverErrors.Clear();

        if (problem.Errors is not null)
        {
            foreach (var (fieldName, errors) in problem.Errors)
            {
                AddFieldErrors(fieldName, errors);
            }
        }

        LoadErrorsFromAdditionalProperties(problem.AdditionalProperties);

        _form?.ValidateAsync();
    }

    private void LoadErrorsFromAdditionalProperties(IDictionary<string, object>? additionalProperties)
    {
        if (additionalProperties != null &&
            additionalProperties.TryGetValue("errors", out var errorsObj) &&
            errorsObj is JObject errorsJson)
        {
            foreach (var property in errorsJson.Properties())
            {
                var errorMessages = property.Value.ToObject<List<string>>();
                if (errorMessages != null)
                {
                    AddFieldErrors(property.Name, errorMessages);
                }
            }
        }
    }

    private void AddFieldErrors(string fieldName, IEnumerable<string> errors)
    {
        var normalizedFieldName = NormalizeFieldName(fieldName);
        var errorMessages = errors.ToList();
        if (errorMessages.Count > 0)
        {
            _serverErrors[normalizedFieldName] = errorMessages;
        }
    }

    private static string NormalizeFieldName(string fieldName)
    {
        return fieldName.Contains('.')
            ? fieldName[(fieldName.LastIndexOf('.') + 1)..]
            : fieldName;
    }

    internal async Task<bool> ValidateAsync()
    {
        if (_form == null)
        {
            return false;
        }

        await _form.ValidateAsync();
        return _form.IsValid;
    }
}
