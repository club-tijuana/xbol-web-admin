using FluentValidation;
using Newtonsoft.Json.Linq;

namespace Odasoft.XBOL.Business.ModelValidators
{
    public abstract class MudServerValidatorBase<T> : AbstractValidator<T> where T : class
    {
        public Dictionary<string, List<string>> ServerErrors { get; } = new(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, string> ApiToClientMap { get; } = new(StringComparer.OrdinalIgnoreCase);

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var cleanPropertyName = propertyName.Contains('.')
                ? propertyName.Substring(propertyName.LastIndexOf('.') + 1)
                : propertyName;

            var errors = new List<string>();

            var result = await ValidateAsync(ValidationContext<T>.CreateWithOptions(
                (T)model,
                x => x.IncludeProperties(cleanPropertyName)));

            if (!result.IsValid)
            {
                errors.AddRange(result.Errors.Select(e => e.ErrorMessage));
            }

            if (ServerErrors.TryGetValue(cleanPropertyName, out var serverErrorList))
            {
                errors.AddRange(serverErrorList);
                ServerErrors.Remove(cleanPropertyName);
            }

            return errors;
        };

        public void LoadServerErrors(ApiException<ProblemDetails> ex)
        {
            ServerErrors.Clear();

            if (ex.Result.AdditionalProperties != null &&
                ex.Result.AdditionalProperties.TryGetValue("errors", out var errorsObj) &&
                errorsObj is JObject errorsJson)
            {
                foreach (var property in errorsJson.Properties())
                {
                    var apiKey = property.Name;

                    if (apiKey.Contains('.'))
                    {
                        apiKey = apiKey.Substring(apiKey.LastIndexOf('.') + 1);
                    }

                    var frontendKey = ApiToClientMap.TryGetValue(apiKey, out var mappedKey)
                        ? mappedKey
                        : apiKey;

                    var errorMessages = property.Value.ToObject<List<string>>();
                    if (errorMessages != null)
                    {
                        ServerErrors[frontendKey] = errorMessages;
                    }
                }
            }
        }
    }
}
