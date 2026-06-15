using FluentValidation;
using Odasoft.XBOL.AdminPortal.ViewModels;

namespace Odasoft.XBOL.AdminPortal.Validators
{
    public class BookingFormFluentValidator : AbstractValidator<BookingForm>
    {
        public BookingFormFluentValidator()
        {
            RuleFor(x => x.PhoneRegionCodeId)
                .NotNull().WithMessage("El codigo de país es requerido.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("El número de teléfono es requerido.")
                .Matches(@"^\d{10}$").WithMessage("El número de teléfono debe tener 10 dígitos.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo electrónico es requerido.")
                .EmailAddress().WithMessage("El correo electrónico no es válido.");

            RuleFor(x => x.TicketType)
                .NotEmpty()
                .WithMessage("El Tipo de entrada es requerido.");

            RuleFor(x => x.CardAmount)
                .GreaterThanOrEqualTo(0).WithMessage("El monto de tarjeta no puede ser negativo.");

            RuleFor(x => x.CashAmount)
                .GreaterThanOrEqualTo(0).WithMessage("El monto de efectivo no puede ser negativo.");

            RuleFor(x => x.DolarAmount)
                .GreaterThanOrEqualTo(0).WithMessage("El monto en dólares no puede ser negativo.");

            RuleFor(x => x.CreditAmount)
                .GreaterThanOrEqualTo(0).WithMessage("El monto de crédito no puede ser negativo.");

            RuleFor(x => x.OtherAmount)
                .GreaterThanOrEqualTo(0).WithMessage("El monto de otros métodos no puede ser negativo.");

            RuleFor(x => x.DeliveryType)
                .NotEmpty().WithMessage("Elige método de entrega");

            RuleFor(x => x.AcceptTermsAndConditions)
                .Equal(true).WithMessage("Debes aceptar los términos y condiciones.");

            RuleFor(x => x)
                .Custom((model, context) =>
                {
                    var totalPaidMx =
                        (model.CardAmount ?? 0)
                        + (model.CashAmount ?? 0)
                        + ((model.DolarAmount ?? 0) * model.ExchangeRate)
                        + (model.CreditAmount ?? 0)
                        + (model.OtherAmount ?? 0);

                    var totalChangeMx =
                        (model.MXNAmount ?? 0)
                        + ((model.USDAmount ?? 0) * model.ExchangeRate);

                    var overpayment =
                        totalPaidMx - model.TotalPrice;

                    if (overpayment <= 0)
                    {
                        return;
                    }

                    var remaining =
                        overpayment - totalChangeMx;

                    if (remaining > 0.01m)
                    {
                        context.AddFailure(
                            nameof(model.MXNAmount),
                            $"Falta devolver {remaining:C2} de cambio.");

                        context.AddFailure(
                            nameof(model.USDAmount),
                            $"Falta devolver {remaining:C2} de cambio.");
                    }

                    if (remaining < -0.01m)
                    {
                        context.AddFailure(
                            nameof(model.MXNAmount),
                            $"El cambio excede por {Math.Abs(remaining ?? 0):C2}.");

                        context.AddFailure(
                            nameof(model.USDAmount),
                            $"El cambio excede por {Math.Abs(remaining ?? 0):C2}.");
                    }
                });
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<BookingForm>.CreateWithOptions((BookingForm)model, x => x.IncludeProperties(propertyName)));

            if (result.IsValid)
            {
                return Array.Empty<string>();
            }

            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
