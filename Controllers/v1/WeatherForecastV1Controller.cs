using System.Text.Json;
using AutoMapper;
using BenchmarkDotNet.Running;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Services.Controllers.API.Configuration;
using Services.Controllers.API.Database.Models;
using Services.Controllers.API.Models;
using Services.Controllers.API.RateLimit;
using Services.Controllers.API.Services;

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
  private readonly ILogger<WeatherForecastController> _logger;
  private readonly IValidator<WeatherForecastDto> _validator;
  private readonly IMapper _mapper;
  private readonly ServicesApiDbRepo _services;

  /// <summary>
  /// Initializes a new instance of the <see cref="WeatherForecastController"/> class.
  /// </summary>
  /// <param name="logger">Logger for the controller.</param>
  /// <param name="validator">IValidator</param>
  /// <param name="mapper">IMapper</param>
  /// <param name="services">ServicesApiDbRepo</param>
  public WeatherForecastController(
    ILogger<WeatherForecastController> logger,
    IValidator<WeatherForecastDto> validator,
    IMapper mapper,
    ServicesApiDbRepo services
  )
  {
    _logger = logger;
    _validator = validator;
    _mapper = mapper;
    _services = services;
  }

  /// <summary>
  /// Benchmark.
  /// </summary>
  /// <remarks>This is a benchmark process.</remarks>> 
  /// <returns>Benchmark results.</returns>
  /// <response code="200">Returns weather forecast paginated list</response>
  /// <response code="500">For a bad request</response>
  [HttpGet("benchmark")]
  // [Tags(["weather-forecast"])]
  [MapToApiVersion("1.0")]
  [EndpointName("Benchmark")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> Benchmark()
  {
    var summary = BenchmarkRunner.Run<MemoryBenchmarkerDemo>();

    return await Task.FromResult<IActionResult>(
      Ok(JsonSerializer.Serialize<string>(summary.Table.ToString()!))
    );
  }

  /// <summary>
  /// Retrieves all weather forecasts paginated.
  /// </summary>
  /// <remarks>This is a paginated WeatherForecast list summary.</remarks>> 
  /// <returns>A paginated list of weather forecasts.</returns>
  /// <response code="200">Returns weather forecast paginated list</response>
  /// <response code="429">Returns to many requests</response>
  /// <response code="500">For a bad request</response>
  [HttpGet("paginated")]
  // [Tags(["weather-forecast"])]
  [MapToApiVersion("1.0")]
  [EndpointName("WeatherForecastPaginated")]
  [ProducesResponseType<PagedResultResponse<WeatherForecastResponse>>(StatusCodes.Status200OK, "application/json")]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/json")]
  public async Task<IActionResult> GetPaginated(
    [FromQuery] PaginationQuery paginationQuery
  )
  {
    PagedResult<WeatherForecastDto> tempResp
      = await _services.FilterPaginationAsync(paginationQuery);

    PagedResultResponse<WeatherForecastResponse> resp
      = _mapper.Map<PagedResultResponse<WeatherForecastResponse>>(tempResp);
    return await Task.FromResult<IActionResult>(Ok(resp));
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
    IQueryable<WeatherForecastDto> forecast = await _services.FilterAsync();

    WeatherForecastResponse[] resp = _mapper.Map<WeatherForecastResponse[]>(forecast);

    return await Task.FromResult<IActionResult>(Ok(resp));
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

    WeatherForecastDto resultDto
      = await _services.FilterAsyncById(id);

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
    newForecast.TemperatureF = 32 + (int)(newForecast.TemperatureC / 0.5556);

    // Validation
    var validationResult = await _validator.ValidateAsync(newForecast);
    if (!validationResult.IsValid)
    {
      // var errors = validationResult.Errors.Select(e => e.ErrorMessage);
      //return Results.BadRequest(errors);
      return BadRequest(validationResult.ToDictionary());
    }

    await _services.CreateAsync(newForecast);

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
      return await Task.FromResult<IActionResult>(
        BadRequest(new ProblemDetails
        {
          Title = "Bad Request",
          Detail = "Id is required.",
          Status = StatusCodes.Status400BadRequest
        })
      );
    }

    newForecast.TemperatureF = 32 + (int)(newForecast.TemperatureC / 0.5556);

    // Validation
    var validationResult = await _validator.ValidateAsync(newForecast);
    if (!validationResult.IsValid)
    {
      // var errors = validationResult.Errors.Select(e => e.ErrorMessage);
      //return Results.BadRequest(errors);
      return BadRequest(validationResult.ToDictionary());
    }

    if (newForecast == null)
    {
      return await Task.FromResult<IActionResult>(
        BadRequest(new ProblemDetails
        {
          Title = "Bad Request",
          Detail = "Weather forecast is required.",
          Status = StatusCodes.Status400BadRequest
        })
      );
    }

    try
    {
      await _services.UpdateAsync(x => x.Id == id, newForecast);
    }
    catch (System.Exception ex)
    {
      return await Task.FromResult<IActionResult>(
        BadRequest(new ProblemDetails
        {
          Title = "Bad Request",
          Detail = ex.Message,
          Status = StatusCodes.Status400BadRequest
        })
      );
    }


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

    IEnumerable<WeatherForecastDto> resultDtoList
      = await _services.FilterAsync(x => x.Id == id);

    WeatherForecastDto? resultDto = resultDtoList.FirstOrDefault();

    if (resultDto is null)
    {
      return await Task.FromResult<IActionResult>(
        NotFound(new ProblemDetails
        {
          Title = "Not Found",
          Detail = "Weather forecast not found.",
          Status = StatusCodes.Status404NotFound
        }));
    }

    await _services.DeleteAsync(resultDto);

    return await Task.FromResult<IActionResult>(NoContent());
  }

}
