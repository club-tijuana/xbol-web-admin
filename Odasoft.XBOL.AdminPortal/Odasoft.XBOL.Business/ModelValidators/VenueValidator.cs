using FluentValidation;

namespace Odasoft.XBOL.Business.ModelValidators
{
    public class VenueValidator : AbstractValidator<VenueRequest>
    {
        public VenueValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Nombre del recinto es requerido.");

            RuleFor(x => x.Category).NotNull().WithMessage("Tipo de recinto es requerido.");

            RuleFor(x => x.Country).NotEmpty().WithMessage("País es requerido.");

            RuleFor(x => x.State).NotEmpty().WithMessage("Estado es requerido.");

            RuleFor(x => x.City).NotEmpty().WithMessage("Ciudad es requerida.");

            RuleFor(x => x.Neighborhood).NotEmpty().WithMessage("Colonia es requerida.");

            RuleFor(x => x.StreetAddress).NotEmpty().WithMessage("Calle es requerida.");

            RuleFor(x => x.ExtNum).NotEmpty().WithMessage("Número exterior es requerido.");

            RuleFor(x => x.ZipCode).NotEmpty().WithMessage("Código postal es requerido.");

            RuleFor(x => x.Latitude).NotNull().WithMessage("Latitud es requerida.");

            RuleFor(x => x.Longitude).NotNull().WithMessage("Longitud es requerida.");

            RuleFor(x => x.ContactEmail).EmailAddress().WithMessage("Correo invalido.");

            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90).WithMessage("La latitud debe ser un valor entre -90 y 90 grados.");

            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180).WithMessage("La longitud debe ser un valor entre -180 y 180 grados.");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<VenueRequest>.CreateWithOptions((VenueRequest)model, x => x.IncludeProperties(propertyName)));

            if (result.IsValid)
            {
                return [];
            }

            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
