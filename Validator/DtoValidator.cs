using FluentValidation;

namespace Services.Controllers.API.Validator;

/// <summary>
/// Validator class for <see cref="WeatherForecast"/> DTO using FluentValidation.
/// Ensures the data integrity of <see cref="WeatherForecast"/> properties.
/// </summary>
public class DtoValidator : AbstractValidator<WeatherForecast>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="DtoValidator"/> class.
  /// Defines validation rules for <see cref="WeatherForecast"/>.
  /// </summary>
  public DtoValidator()
  {
    RuleFor(x => x.Date)
        .NotNull()
        .NotEmpty()
        .WithMessage("Date is required");

    RuleFor(x => x.TemperatureC)
        .NotNull()
        .NotEmpty()
        .WithMessage("TemperatureC is required");
  }
}
