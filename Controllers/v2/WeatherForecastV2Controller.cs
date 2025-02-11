using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Services.Controllers.API.Database.Models;
using Services.Controllers.API.RateLimit;
using Services.Controllers.API.Services;

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
  private readonly ILogger<WeatherForecastController> _logger;
  private readonly ServicesApiDbRepo _services;

  /// <summary>
  /// Initializes a new instance of the <see cref="WeatherForecastController"/> class.
  /// </summary>
  /// <param name="logger">Logger for the controller.</param>
  /// <param name="services">ServicesApiDbRepo</param>
  public WeatherForecastController(
    ILogger<WeatherForecastController> logger,
    ServicesApiDbRepo services
  )
  {
    _logger = logger;
    _services = services;
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
    // if (string.IsNullOrWhiteSpace(id))
    // {
    //   return await Task.FromResult<IActionResult>(BadRequest(new ProblemDetails
    //   {
    //     Title = "Bad Request",
    //     Detail = "Id is required.",
    //     Status = StatusCodes.Status400BadRequest
    //   }));
    // }

    // WeatherForecastDto? resultDto = await _services.FilterAsync(id);

    // if (resultDto is null)
    // {
    //   return await Task.FromResult<IActionResult>(
    //     NotFound(new ProblemDetails
    //     {
    //       Title = "Not Found",
    //       Detail = "Weather forecast not found.",
    //       Status = StatusCodes.Status404NotFound
    //     }));
    // }

    // await _services.DeleteAsync(resultDto);

    return await Task.FromResult<IActionResult>(NoContent());
  }

}
