using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Services.Controllers.API.Database.Models;
using Services.Controllers.API.Models;
using Services.Controllers.API.RateLimit;

namespace Services.Controllers.API.Controllers.v1;

/// <summary>
/// Controller for managing weather forecasts.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
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
        Date = DateTime.Now,
        TemperatureC = 25,
        Summary = "Hot"
      },
      new WeatherForecastDto {
        Id = "3db3a34a-9dcf-42e6-977f-d6bbb2329f16",
        Date = DateTime.Now,
        TemperatureC = 15,
        Summary = "Cool"
      },
      new WeatherForecastDto {
        Id = "76d5e039-63b3-4c7f-bb8d-0847f729dcde",
        Date = DateTime.Now,
        TemperatureC = 5,
        Summary = "Cold"
      },
      new WeatherForecastDto {
        Id = "1130f076-1d75-4977-8a50-323a4ecf8f4e",
        Date = DateTime.Now,
        TemperatureC = 35,
        Summary = "Very Hot"
      },
      new WeatherForecastDto {
        Id = "2fa8d533-c8fd-45e6-8ee4-988e5b1d8d04",
        Date = DateTime.Now,
        TemperatureC = 20,
        Summary = "Warm"
      }
     ];

  private readonly ILogger<WeatherForecastController> _logger;
  private readonly IValidator<WeatherForecastDto> _validator;
  private readonly IMapper _mapper;

  /// <summary>
  /// Initializes a new instance of the <see cref="WeatherForecastController"/> class.
  /// </summary>
  /// <param name="logger">Logger for the controller.</param>
  /// <param name="validator">IValidator</param>
  /// <param name="mapper">IMapper</param>
  public WeatherForecastController(
    ILogger<WeatherForecastController> logger,
    IValidator<WeatherForecastDto> validator,
    IMapper mapper
  )
  {
    _logger = logger;
    _validator = validator;
    _mapper = mapper;
  }

  /// <summary>
  /// Retrieves all weather forecasts.
  /// </summary>
  /// <remarks>This is a WeatherForecast list summary.</remarks>> 
  /// <returns>A list of weather forecasts.</returns>
  /// <response code="200">Returns weather forecast list</response>
  /// <response code="429">Returns to many requests</response>
  /// <response code="500">For a bad request</response>
  [HttpGet()]
  // [Tags(["weather-forecast"])]
  [MapToApiVersion("1.0")]
  [EndpointName("WeatherForecast")]
  [ProducesResponseType<IEnumerable<WeatherForecastResponse>>(StatusCodes.Status200OK, "application/json")]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/json")]
  // [ApiExplorerSettings(IgnoreApi = true)]  
  public async Task<IActionResult> Get()
  {
    WeatherForecastResponse[] resp = _mapper.Map<WeatherForecastResponse[]>(forecast);

    return await Task.FromResult<IActionResult>(Ok(forecast));
  }


  /// <summary>
  /// Retrieves a specific weather forecast by ID.
  /// </summary>
  /// <remarks>This is a WeatherForecast summary.</remarks>> 
  /// <param name="id">The unique identifier of the weather forecast.</param>
  /// <returns>The requested weather forecast if found.</returns>
  /// <response code="200">Returns weather forecast if found</response>
  /// <response code="400">Returns bad request status</response>
  /// <response code="404">Returns nothing if not found</response>
  /// <response code="429">Returns to many requests</response>
  /// <response code="500">For internal server error</response>
  [HttpGet("{id}")]
  // [Tags(["weather-forecast"])]
  [MapToApiVersion("1.0")]
  [EndpointName("WeatherForecastById")]
  [ProducesResponseType<WeatherForecastResponse>(StatusCodes.Status200OK, "application/json")]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/json")]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/json")]
  // [ApiExplorerSettings(IgnoreApi = true)]  
  public async Task<IActionResult> GetById(string id)
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

    WeatherForecastDto? resultDto = forecast.FirstOrDefault(x => x.Id == id);

    if (resultDto == null)
    {
      return await Task.FromResult<IActionResult>(
        NotFound(new ProblemDetails
        {
          Title = "Not Found",
          Detail = "Weather forecast not found.",
          Status = StatusCodes.Status404NotFound
        }));
    }

    WeatherForecastResponse result = _mapper.Map<WeatherForecastResponse>(resultDto);

    return Ok(result);
  }


  /// <summary>
  /// Create a new WeatherForecast 
  /// </summary>
  /// <param name="newForecast">WeatherForecast</param>
  /// <returns>A newly created weather forecast</returns>
  /// <remarks>
  /// This is a WeatherForecast create summary.
  /// Sample request:
  ///
  ///     POST /WeatherForecast
  ///     {
  ///        "id": 1,
  ///        "date": "2025-01-14",
  ///        "temperatureC": 0,
  ///        "temperatureF": 0,
  ///        "summary": "string"
  ///     }
  ///
  /// </remarks>
  /// <response code="201">Returns the newly created item</response>
  /// <response code="400">If the item is null</response>
  /// <response code="429">Returns to many requests</response>
  /// <response code="500">For internal server error</response>
  [HttpPost]
  // [Tags(["weather-forecast"])]
  [MapToApiVersion("1.0")]
  [EndpointName("WeatherForecastCreate")]
  [ProducesResponseType<WeatherForecastResponse>(StatusCodes.Status201Created, "application/json")]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/json")]
  // [ApiExplorerSettings(IgnoreApi = true)]
  public async Task<IActionResult> Create(
    [FromBody] WeatherForecastDto newForecast
  )
  {
    // Validation
    var validationResult = await _validator.ValidateAsync(newForecast);
    if (!validationResult.IsValid)
    {
      // var errors = validationResult.Errors.Select(e => e.ErrorMessage);
      //return Results.BadRequest(errors);
      return BadRequest(validationResult.ToDictionary());
    }

    forecast = [.. forecast, newForecast];

    return await Task.FromResult<IActionResult>(
      CreatedAtAction(
        nameof(GetById),
        new { id = newForecast.Id },
        newForecast)
      );
  }

  /// <summary>
  /// Updates an existing weather forecast.
  /// </summary>
  /// <param name="id">The ID of the forecast to update.</param>
  /// <param name="newForecast">The updated weather forecast details.</param>
  /// <returns>No content if update is successful.</returns>
  /// <remarks>
  /// This is a WeatherForecast update summary.
  /// Sample request:
  ///
  ///     PUT /WeatherForecast
  ///     {
  ///        "id": 1,
  ///        "date": "2025-01-14",
  ///        "temperatureC": 0,
  ///        "temperatureF": 0,
  ///        "summary": "string"
  ///     }
  ///
  /// </remarks>
  /// <response code="204">Returns no content if succeeded</response>
  /// <response code="400">If the item is null</response>
  /// <response code="404">If the item is null</response>
  /// <response code="429">Returns to many requests</response>
  /// <response code="500">For internal server error</response>
  [HttpPut("{id}")]
  // [Tags(["weather-forecast"])]
  [MapToApiVersion("1.0")]
  [EndpointName("WeatherForecastUpdate")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/json")]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/json")]
  // [ApiExplorerSettings(IgnoreApi = true)]
  public async Task<IActionResult> Update(
    string id,
    [FromBody] WeatherForecastDto newForecast
  )
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

    // Validation
    var validationResult = await _validator.ValidateAsync(newForecast);
    if (!validationResult.IsValid)
    {
      // var errors = validationResult.Errors.Select(e => e.ErrorMessage);
      //return Results.BadRequest(errors);
      return BadRequest(validationResult.ToDictionary());
    }

    if (forecast == null)
    {
      return await Task.FromResult<IActionResult>(BadRequest(new ProblemDetails
      {
        Title = "Bad Request",
        Detail = "Weather forecast is required.",
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

    result.Date = newForecast.Date;
    result.TemperatureC = newForecast.TemperatureC;
    result.Summary = newForecast.Summary;

    return await Task.FromResult<IActionResult>(NoContent());
  }

  /// <summary>
  /// Deletes a weather forecast by ID.
  /// </summary>
  /// <remarks>
  /// This is a WeatherForecast delete summary.
  /// Sample request:
  /// 
  ///     DELETE /WeatherForecast/38b7942a-8a8f-4a34-9744-e4dea6eaed78 
  /// 
  /// </remarks>
  /// <param name="id">The ID of the forecast to delete.</param>
  /// <returns>No content if deletion is successful.</returns>
  /// <response code="204">Returns no content if succeeded</response>
  /// <response code="400">If the item is null</response>
  /// <response code="404">If the item is null</response>
  /// <response code="429">Returns to many requests</response>
  /// <response code="500">For internal server error</response>
  [HttpDelete("{id}")]
  // [Tags(["weather-forecast"])]
  [MapToApiVersion("1.0")]
  [EndpointName("WeatherForecastDeleteV1")]
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
