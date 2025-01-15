using Microsoft.AspNetCore.Mvc;

namespace Services.Controllers.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
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

  public WeatherForecast[] forecast = [
      new WeatherForecast {
        Id = "38b7942a-8a8f-4a34-9744-e4dea6eaed78",
        Date = DateOnly.FromDateTime(DateTime.Now),
        TemperatureC = 25,
        Summary = "Hot"
      },
      new WeatherForecast {
        Id = "3db3a34a-9dcf-42e6-977f-d6bbb2329f16",
        Date = DateOnly.FromDateTime(DateTime.Now),
        TemperatureC = 15,
        Summary = "Cool"
      },
      new WeatherForecast {
        Id = "76d5e039-63b3-4c7f-bb8d-0847f729dcde",
        Date = DateOnly.FromDateTime(DateTime.Now),
        TemperatureC = 5,
        Summary = "Cold"
      },
      new WeatherForecast {
        Id = "1130f076-1d75-4977-8a50-323a4ecf8f4e",
        Date = DateOnly.FromDateTime(DateTime.Now),
        TemperatureC = 35,
        Summary = "Very Hot"
      },
      new WeatherForecast {
        Id = "2fa8d533-c8fd-45e6-8ee4-988e5b1d8d04",
        Date = DateOnly.FromDateTime(DateTime.Now),
        TemperatureC = 20,
        Summary = "Warm"
      }
    ];

  private readonly ILogger<WeatherForecastController> _logger;

  public WeatherForecastController(ILogger<WeatherForecastController> logger)
  {
    _logger = logger;
  }

  [HttpGet()]
  [Tags(["weather-forecast"])]
  [EndpointName("WeatherForecast")]
  [EndpointSummary("This is a WeatherForecast list summary.")]
  [EndpointDescription("This is a WeatherForecast list description.")]
  [ProducesResponseType<IEnumerable<WeatherForecast>>(StatusCodes.Status200OK, "application/json", ["application/xml"])]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/json")]
  // [ApiExplorerSettings(IgnoreApi = true)]  
  public async Task<IActionResult> Get()
  {
    return await Task.FromResult<IActionResult>(Ok(forecast));
  }


  [HttpGet("{id}")]
  [Tags(["weather-forecast"])]
  [EndpointName("WeatherForecastById")]
  [EndpointSummary("This is a WeatherForecast summary.")]
  [EndpointDescription("This is a WeatherForecast description.")]
  [ProducesResponseType<WeatherForecast>(StatusCodes.Status200OK, "application/json")]
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

    WeatherForecast? result = forecast.FirstOrDefault(x => x.Id == id);

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

    return Ok(result);
  }
  
  /// <summary>
  /// Create a new WeatherForecast 
  /// </summary>
  /// <param name="newForecast">WeatherForecast</param>
  /// <returns>A newly created weather forecast</returns>
  /// <remarks>
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
  /// <response code="500">For a bad request</response>
  [HttpPost]
  [Tags(["weather-forecast"])]
  [EndpointName("WeatherForecastCreate")]
  [EndpointSummary("This is a WeatherForecast create summary.")]
  [EndpointDescription("This is a WeatherForecast create description.")]
  [ProducesResponseType<WeatherForecast>(StatusCodes.Status201Created, "application/json")]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/json")]
  // [ApiExplorerSettings(IgnoreApi = true)]
  public async Task<IActionResult> Create(
    [FromBody] WeatherForecast newForecast
  )
  {
    if (newForecast == null)
    {
      return await Task.FromResult<IActionResult>(BadRequest(new ProblemDetails
      {
        Title = "Bad Request",
        Detail = "Weather forecast is required.",
        Status = StatusCodes.Status400BadRequest
      }));
    }

    //forecast.Id = Guid.NewGuid().ToString();
    newForecast.Date = DateOnly.FromDateTime(DateTime.Now);
    forecast = [.. forecast, newForecast];

    return await Task.FromResult<IActionResult>(
      CreatedAtAction(
        nameof(GetById),
        new { id = newForecast.Id },
        newForecast)
      );
  }

  [HttpPut("{id}")]
  [Tags(["weather-forecast"])]
  [EndpointName("WeatherForecastUpdate")]
  [EndpointSummary("This is a WeatherForecast update summary.")]
  [EndpointDescription("This is a WeatherForecast update description.")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/json")]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/json")]
  // [ApiExplorerSettings(IgnoreApi = true)]
  public async Task<IActionResult> Update(
    string id,
    [FromBody] WeatherForecast newForecast
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

    if (forecast == null)
    {
      return await Task.FromResult<IActionResult>(BadRequest(new ProblemDetails
      {
        Title = "Bad Request",
        Detail = "Weather forecast is required.",
        Status = StatusCodes.Status400BadRequest
      }));
    }

    WeatherForecast? result = forecast.FirstOrDefault(x => x.Id == id);

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

  [HttpDelete("{id}")]
  [Tags(["weather-forecast"])]
  [EndpointName("WeatherForecastDelete")]
  [EndpointSummary("This is a WeatherForecast delete summary.")]
  [EndpointDescription("This is a WeatherForecast delete description.")]
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

    WeatherForecast? result = forecast.FirstOrDefault(x => x.Id == id);

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
