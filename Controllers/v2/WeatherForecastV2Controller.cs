using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Services.Controllers.API.Models;
using Services.Controllers.API.RateLimit;

namespace Services.Controllers.API.Controllers.v2;

/// <summary>
/// Controller for managing weather forecasts.
/// </summary>
[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[EnableRateLimiting(CommonRateLimitExtension.FixedPolicy!)]
// [Authorize]
public class WeatherForecastController : ControllerBase
{
  /// <summary>
  /// Static list of weather summaries.
  /// </summary>
  private static readonly string[] Summaries = new[]
   {
    "Freezing",
    "Bracing",
    "Chilly",
    "Cool",
    "Mild",
    "Warm",
    "Balmy",
    "Hot",
    "Sweltering",
    "Scorching"
  };

  /// <summary>
  /// In-memory storage for weather forecasts.
  /// </summary>
  public WeatherForecastDto[] forecast = [
       new WeatherForecastDto {
        Id = "38b7942a-8a8f-4a34-9744-e4dea6eaed78",
        Date =DateTime.Now,
        TemperatureC = 25,
        Summary = "Hot"
      },
      new WeatherForecastDto {
        Id = "3db3a34a-9dcf-42e6-977f-d6bbb2329f16",
        Date =DateTime.Now,
        TemperatureC = 15,
        Summary = "Cool"
      },
      new WeatherForecastDto {
        Id = "76d5e039-63b3-4c7f-bb8d-0847f729dcde",
        Date =DateTime.Now,
        TemperatureC = 5,
        Summary = "Cold"
      },
      new WeatherForecastDto {
        Id = "1130f076-1d75-4977-8a50-323a4ecf8f4e",
        Date =DateTime.Now,
        TemperatureC = 35,
        Summary = "Very Hot"
      },
      new WeatherForecastDto {
        Id = "2fa8d533-c8fd-45e6-8ee4-988e5b1d8d04",
        Date =DateTime.Now,
        TemperatureC = 20,
        Summary = "Warm"
      }
     ];

  private readonly ILogger<WeatherForecastController> _logger;

  /// <summary>
  /// Initializes a new instance of the <see cref="WeatherForecastController"/> class.
  /// </summary>
  /// <param name="logger">Logger for the controller.</param>
  public WeatherForecastController(ILogger<WeatherForecastController> logger)
  {
    _logger = logger;
  }

  /// <summary>
  /// Deletes a weather forecast by ID.
  /// </summary>
  /// <remarks>This is a WeatherForecast delete summary.</remarks>
  /// <param name="id">The ID of the forecast to delete.</param>
  /// <returns>No content if deletion is successful.</returns>
  /// <response code="204">Returns no content if succeeded</response>
  /// <response code="400">If the item is null</response>
  /// <response code="404">If the item is null</response>
  /// <response code="429">Returns to many requests</response>
  /// <response code="500">For internal server error</response>
  [HttpDelete("{id}")]
  // [Tags(["weather-forecast"])]
  [MapToApiVersion("2.0")]
  [EndpointName("WeatherForecastDelete")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/json")]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/json")]
  // [ApiExplorerSettings(IgnoreApi = true)]
  public async Task<IActionResult> Delete(string id)
  {
    if (string.IsNullOrWhiteSpace(id))
    {
      return await Task.FromResult<IActionResult>(BadRequest(new ProblemDetails
      {
        Title = "Bad Request",
        Detail = "Id is required.",
        Status = StatusCodes.Status400BadRequest
      }));
    }

    WeatherForecastDto? result = forecast.FirstOrDefault(x => x.Id == id);

    if (result == null)
    {
      return await Task.FromResult<IActionResult>(
        NotFound(new ProblemDetails
        {
          Title = "Not Found",
          Detail = "Weather forecast not found.",
          Status = StatusCodes.Status404NotFound
        }));
    }

    forecast = forecast.Where(x => x.Id != id).ToArray();

    return await Task.FromResult<IActionResult>(NoContent());
  }

}
